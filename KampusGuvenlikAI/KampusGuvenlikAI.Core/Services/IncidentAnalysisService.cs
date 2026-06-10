using System.Net.Http.Headers;
using System.Text;
using KampusGuvenlikAI.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KampusGuvenlikAI.Core.Services;

public class IncidentAnalysisService
{
    private readonly AppSettings _settings;

    public IncidentAnalysisService(AppSettings settings)
    {
        _settings = settings;
    }

    public async Task<AnalysisResult> AnalyzeAsync(string reportText)
    {
        var cleanedText = TextSanitizer.CleanIncidentText(reportText);

        if (string.IsNullOrWhiteSpace(cleanedText))
        {
            throw new ArgumentException("Olay metni bos olamaz.");
        }

        try
        {
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
            {
                if (_settings.UseRuleBasedFallback)
                {
                    return AnalyzeWithRules(cleanedText, "API anahtari girilmedigi icin yerel kural tabanli analiz kullanildi.");
                }

                throw new InvalidOperationException("API anahtari tanimli degil. Ayarlar ekranindan API Key giriniz.");
            }

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ApiKey);

            var body = new
            {
                model = _settings.Model,
                response_format = new { type = "json_object" },
                messages = new object[]
                {
                    new
                    {
                        role = "system",
                        content = "Kampus guvenlik olay raporlarini analiz eden bir asistansin. Sadece JSON don: category, urgency, steps alanlari olsun. category: Guvenlik, Saglik, Disiplin, Kayip Esya, Yangin, Teknik Ariza veya Diger. urgency: Dusuk, Orta veya Yuksek. steps uc maddelik string dizisi olsun."
                    },
                    new
                    {
                        role = "user",
                        content = cleanedText
                    }
                }
            };

            var json = JsonConvert.SerializeObject(body);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync(_settings.ApiUrl, content);
            var responseText = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return ParseOpenAiResponse(responseText);
        }
        catch (Exception ex)
        {
            Logger.Log(ex);

            if (_settings.UseRuleBasedFallback)
            {
                return AnalyzeWithRules(cleanedText, "API baglantisi basarisiz oldugu icin yerel kural tabanli analiz kullanildi.");
            }

            throw;
        }
    }

    private static AnalysisResult ParseOpenAiResponse(string responseText)
    {
        var root = JObject.Parse(responseText);
        var content = root["choices"]?[0]?["message"]?["content"]?.ToString() ?? "{}";
        var parsed = JObject.Parse(content);
        var steps = parsed["steps"] as JArray ?? [];

        return new AnalysisResult
        {
            Category = parsed["category"]?.ToString() ?? "Diger",
            Urgency = parsed["urgency"]?.ToString() ?? "Orta",
            Step1 = steps.ElementAtOrDefault(0)?.ToString() ?? "Olay yeri guvenligini sagla.",
            Step2 = steps.ElementAtOrDefault(1)?.ToString() ?? "Ilgili birime haber ver.",
            Step3 = steps.ElementAtOrDefault(2)?.ToString() ?? "Raporu sistemde kayit altina al.",
            RawResponse = responseText
        };
    }

    private static AnalysisResult AnalyzeWithRules(string text, string rawResponse)
    {
        var category = "Diger";
        var urgency = "Orta";

        if (ContainsAny(text, "BIcak", "SILAH", "KAVGA", "HIRSIZLIK", "SALDIRI", "TEHDIT"))
        {
            category = "Guvenlik";
            urgency = "Yuksek";
        }
        else if (ContainsAny(text, "BAYIL", "YARAL", "AMBULANS", "KANAMA", "HASTA", "KALP"))
        {
            category = "Saglik";
            urgency = "Yuksek";
        }
        else if (ContainsAny(text, "KURAL", "DISIPLIN", "TARTISMA", "GURULTU", "KOPYA"))
        {
            category = "Disiplin";
            urgency = "Orta";
        }
        else if (ContainsAny(text, "KAYIP", "CUZDAN", "TELEFON", "KIMLIK", "CANTA"))
        {
            category = "Kayip Esya";
            urgency = "Dusuk";
        }
        else if (ContainsAny(text, "YANGIN", "DUMAN", "GAZ", "PATLAMA"))
        {
            category = "Yangin";
            urgency = "Yuksek";
        }
        else if (ContainsAny(text, "ELEKTRIK", "KAPI", "KAMERA", "ARIZA", "SU BASKINI"))
        {
            category = "Teknik Ariza";
            urgency = "Orta";
        }

        return new AnalysisResult
        {
            Category = category,
            Urgency = urgency,
            Step1 = "Olay yerini kontrol altina al ve riskleri azalt.",
            Step2 = "Ilgili kampus birimine ve gerekiyorsa acil yardima haber ver.",
            Step3 = "Tanik, saat, konum ve delil bilgilerini rapora ekle.",
            RawResponse = rawResponse
        };
    }

    private static bool ContainsAny(string text, params string[] keywords)
    {
        return keywords.Any(keyword => text.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }
}
