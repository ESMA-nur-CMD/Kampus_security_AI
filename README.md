# Kampus Guvenlik Olay Raporu Siniflandirma

Bu cozum, C# Windows Forms App (.NET 8 Windows) ve ayri bir Class Library projesi ile hazirlanmis iki katmanli bir kampus guvenlik olay yonetim sistemidir.

## Proje Yapisi

- `KampusGuvenlikAI.WinForms`: Formlari ve kullanici arayuzunu barindirir.
- `KampusGuvenlikAI.Core`: Is mantigi, modeller, SQLite veri tabani islemleri, API/AI analiz servisi, ayarlar ve loglama siniflarini barindirir.
- Ana uygulama `KampusGuvenlikAI.Core.dll` referansini kullanir. DLL, build sonrasinda `KampusGuvenlikAI.Core/bin/Debug/net8.0/KampusGuvenlikAI.Core.dll` altinda uretilir.

## Formlar ve Kontroller

### Form1 - Ana Menu / Dashboard

- `Button`: Yeni Olay Raporu Gir
- `Button`: Gecmis Raporlari Listele & Karsilastir
- `Button`: Sistem Ayarlari (API/DB)
- `PictureBox`: Ana menu arka plan/gorsel alani
- Gorevi: Sayfalar arasi temiz yonlendirme saglar. `KampusGuvenlikAI.WinForms/Assets/kmu_nizamiye.jpg` dosyasi varsa ana menude otomatik gosterilir.

### Form2 - Yeni Rapor Girisi ve AI Analizi

- `RichTextBox`: Olay metni girisi
- `ComboBox`: Guvenlik gorevlisinin manuel kategorisi, yani `HumanLabel`
- `Button`: Analiz Et
- `Button`: API Test
- `Button`: Kaydet
- `TextBox`: AI kategorisi
- `TextBox`: Aciliyet seviyesi
- `TextBox`: Ilk 3 adim
- Gorevi: Metni `Trim`, `Replace`, `ToUpperInvariant` gibi hazir string metotlariyla temizler, AI servisine gonderir ve sonucu SQLite veri tabanina kaydeder.

### Form3 - Listeleme ve Karsilastirma

- `DataGridView`: Tum raporlar, `HumanLabel`, `AiCategory`, `Urgency` ve karsilastirma durumu
- `ComboBox`: Secili kaydin `HumanLabel` degerini degistirme
- `Button`: Yenile
- `Button`: Guncelle
- `Button`: Sil
- Gorevi: CRUD islemlerini ve insan etiketi ile AI tahmininin yan yana incelenmesini saglar.

### Form4 - Sistem Ayarlari

- `TextBox`: API Key
- `TextBox`: API URL
- `TextBox`: Model
- `TextBox`: SQLite DB Path
- `CheckBox`: API hatasinda yerel kural tabanli analiz kullan
- `Button`: Kaydet
- `Button`: DB Test

## NuGet Paketleri

`KampusGuvenlikAI.Core` projesinde:

- `Newtonsoft.Json`: API JSON request/response islemleri ve ayar dosyasi serilestirme
- `Microsoft.Data.Sqlite`: Lokal SQLite veri tabani CRUD islemleri

## Hata Yonetimi

API, veri tabani ve bos giris gibi riskli noktalar `try-catch-finally` bloklari ile sarilmistir. Hatalar:

- Kullaniciya `MessageBox.Show` ile kurumsal mesaj olarak gosterilir.
- `Logger` sinifi ile `%LocalAppData%/KampusGuvenlikAI/application.log` dosyasina yazilir.

## Calistirma

```powershell
dotnet build .\KampusGuvenlikAI.WinForms\KampusGuvenlikAI.WinForms.csproj
dotnet run --project .\KampusGuvenlikAI.WinForms\KampusGuvenlikAI.WinForms.csproj
```

API Key girilmezse ve ayarlarda fallback acik kalirsa sistem kural tabanli yerel analizle calisir. Gercek API kullanimi icin `Sistem Ayarlari` ekranindan API Key girilmelidir.
