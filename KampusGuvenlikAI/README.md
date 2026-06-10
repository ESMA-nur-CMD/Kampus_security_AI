# Kampus Guvenlik AI - Sade WinForms Surumu

Bu proje C# Windows Forms ile hazirlanmis sade bir kampus guvenlik olay raporu uygulamasidir. Kodlar karmasik `Models`, `Services` gibi katmanlara ayrilmadan dogrudan form kodlarinda tutulmustur.

## Formlar

- `Form1`: Ana menu. KMU nizamiye fotografi `BackgroundImage` olarak kullanilir.
- `Form2`: Yeni olay raporu girisi, OpenAI API analizi ve SQLite INSERT islemi.
- `Form3`: SQLite SELECT ile rapor listeleme, HumanLabel guncelleme ve silme islemleri.
- `Form4`: API Key, API URL, model, SQLite DB yolu ve yedek analiz ayarlari.

## API

`Form2`, `Form4` uzerinden kaydedilen API Key bilgisini okur ve su adrese HTTP POST istegi atar:

```text
https://api.openai.com/v1/chat/completions
```

Gelen JSON cevap `Newtonsoft.Json` ile cozulur ve kategori, aciliyet, ilk 3 adim alanlarina yazilir.

## Veri Tabani

SQLite kullanilir. Kayitlar `IncidentReports` tablosuna basit SQL komutlariyla eklenir, listelenir, guncellenir ve silinir.

## NuGet Paketleri

- `Newtonsoft.Json`
- `Microsoft.Data.Sqlite`

## Calistirma

Visual Studio ile `KampusGuvenlikAI.sln` dosyasini acin veya terminalden:

```powershell
dotnet run --project .\KampusGuvenlikAI.WinForms\KampusGuvenlikAI.WinForms.csproj
```
