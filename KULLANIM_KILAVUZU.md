# Kampus Güvenlik AI - Kurulum ve Kullanım Kılavuzu

## 📋 Proje Özeti

**Kampus Güvenlik Olay Raporu Sınıflandırma Sistemi**, kampus güvenliğinde meydana gelen olayları raporlamak, yapay zeka ile analiz etmek ve yönetmek için geliştirilmiş bir Windows Forms uygulamasıdır.

### 🎯 Temel Özellikler

1. **Hava Durumu Widget'ı** - Karaman'ın gerçek zamanlı hava durumunu gösterir
2. **AI Analiz Butonu** - OpenAI API ile otomatik olay analizi
3. **Aktif Olaylar Paneli** - SQLite veritabanında raporları yönetme
4. **İstatistik Kartları** - Ani istatistik görünümü (bu hafta, son olay, kritik sayısı)

---

## 🚀 Kurulum

### Gereksinimler
- **.NET 8.0** (Windows Desktop)
- **Visual Studio 2022** veya daha yenisi (isteğe bağlı)
- İnternet bağlantısı (API entegrasyonları için)

### Adım 1: Projeyi Klonla
```bash
git clone https://github.com/ESMA-nur-CMD/Kampus_security_AI.git
cd Kampus_security_AI
```

### Adım 2: Bağımlılıkları Yükle
```bash
dotnet restore
```

### Adım 3: Uygulamayı Çalıştır
```bash
dotnet run --project .\KampusGuvenlikAI.WinForms\KampusGuvenlikAI.WinForms.csproj
```

---

## 🔑 API Anahtarları

Uygulamayı tam işlevsellikle kullanmak için aşağıdaki API anahtarlarını almalısınız:

### OpenWeatherMap API Key
1. https://openweathermap.org/api adresine gidin
2. Ücretsiz API Key alın
3. Uygulamada **Sistem Ayarları** → **OpenWeatherMap API Key** alanına yapıştırın

### OpenAI API Key
1. https://platform.openai.com/api-keys adresine gidin
2. Yeni API Key oluşturun
3. Uygulamada **Sistem Ayarları** → **OpenAI API Key** alanına yapıştırın

---

## 📱 Kullanım Kılavuzu

### Form1 - Ana Menü (Dashboard)

**Hava Durumu Widget'ı:**
- Karaman'ın sıcaklık, nem ve rüzgar bilgisi gösterilir
- Her açılışta otomatik güncellenir
- API hatası varsa fallback mekanizması devreye girer

**İstatistik Kartları:**
- **Bu Hafta:** Son 7 gündeki olay sayısı
- **Son Olay:** En son bildirilen olayın zamanı
- **Kritik:** Aciliyet seviyesi "Kritik" olan olay sayısı

**Butonlar:**
- 🔵 **Yeni Olay Raporu Gir:** Form2'ye geçiş
- 🟢 **Geçmiş Raporları Listele:** Form3'e geçiş
- 🟡 **Sistem Ayarları:** Form4'e geçiş

---

### Form2 - Yeni Rapor Girişi ve AI Analizi

**Adım 1: Olay Açıklaması**
- Metin kutusuna olayı ayrıntılı şekilde yazın
- Örnek: "Kütüphane binasının 2. katında yangın çıktı"

**Adım 2: Kategori Seçimi**
- Dropdown menüden manuel kategori seçin (Hırsızlık, Şiddet, Yangın, Tıbbi, Diğer)

**Adım 3: Analiz Et Butonu**
- OpenAI API'ye metni gönder
- AI otomatik olarak şunları belirler:
  - **AI Kategorisi:** Yapay zeka tarafından belirlenen kategori
  - **Aciliyet Seviyesi:** Düşük, Orta, Yüksek, Kritik
  - **İlk 3 Adım:** Yapılması gereken ilk 3 acil adım

**Adım 4: Kaydet Butonu**
- Rapor SQLite veritabanına kaydedilir
- Form temizlenir

**Ek Butonlar:**
- **API Test:** OpenAI bağlantısını test et
- **Kaydet:** Veri tabanına raporu kaydet

---

### Form3 - Listeleme ve Karşılaştırma

**Filtreler:**
- **Tarih Filtresi:** "Tümü", "Bu Hafta", "Bu Ay"
- **Aciliyet Filtresi:** "Tümü", "Düşük", "Orta", "Yüksek", "Kritik"

**DataGridView:**
- Tüm raporları tablosal formatta gösterir
- Sütunlar: ID, Olay Açıklaması, İnsan Etiketi, AI Kategorisi, Aciliyet, Tarih

**Butonlar:**
- **Yenile:** Tabloyu yenile
- **Güncelle:** Seçili raporun İnsan Etiketini güncelle
- **Sil:** Seçili raporu sil (onay gerekli)

---

### Form4 - Sistem Ayarları

**API Ayarları:**
- OpenWeatherMap API Key (şifreli)
- OpenAI API Key (şifreli)
- OpenAI Model (varsayılan: gpt-3.5-turbo)

**Veritabanı Ayarları:**
- SQLite DB Path: Veritabanı dosyasının konumu
- **Gözat:** Klasör seçim diyaloğu

**Seçenekler:**
- ☑️ API hatasında yerel kural tabanlı analiz kullan

**Test Butonları:**
- **OpenWeatherMap Test:** Hava durumu API'sini test et
- **OpenAI Test:** OpenAI bağlantısını test et
- **DB Test:** Veritabanı bağlantısını test et

**Kaydet Butonu:**
- Tüm ayarları `%LocalAppData%/KampusGuvenlikAI/settings.json` dosyasına kaydeder

---

## 🗄️ Veritabanı Şeması

### IncidentReports Tablosu

```sql
CREATE TABLE IncidentReports (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    EventDescription TEXT NOT NULL,
    HumanLabel TEXT,
    AiCategory TEXT,
    Urgency TEXT,
    FirstThreeSteps TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

**Sütunlar:**
- **Id:** Benzersiz tanımlayıcı
- **EventDescription:** Olay metni (zorunlu)
- **HumanLabel:** Güvenlik görevlisinin kategorisi
- **AiCategory:** Yapay zekanın belirlediği kategori
- **Urgency:** Aciliyet seviyesi (Düşük/Orta/Yüksek/Kritik)
- **FirstThreeSteps:** Yapılması gereken ilk 3 adım
- **CreatedAt:** Oluşturulma tarihi
- **UpdatedAt:** Güncellenme tarihi

---

## 📦 NuGet Paketleri

### KampusGuvenlikAI.Core
```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
<PackageReference Include="Microsoft.Data.Sqlite" Version="8.0.0" />
```

### KampusGuvenlikAI.WinForms
```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
<PackageReference Include="Microsoft.Data.Sqlite" Version="8.0.0" />
```

---

## 🛠️ Hata Yönetimi

### Exception Handling
- Tüm API çağrıları `try-catch` blokları ile korunur
- Veritabanı işlemleri güvenli şekilde yönetilir
- Kullanıcıya dostu hata mesajları gösterilir

### Loglama
- Tüm hatalar `%LocalAppData%/KampusGuvenlikAI/application.log` dosyasına yazılır
- Hata türü: [HATA], [INFO], [UYARI]
- Tarih-saat formatı: yyyy-MM-dd HH:mm:ss

### Fallback Mekanizması
- OpenAI API başarısız olursa yerel kural tabanlı analiz devreye girer
- OpenWeatherMap API başarısız olursa "API Hatası" gösterilir
- Veritabanı bağlantısı başarısız olursa işlem durdurulur

---

## 🔄 Veri Akışı

```
Form2 (Rapor Girişi)
    ↓
OpenAI API (Analiz)
    ↓
DatabaseService (Kaydetme)
    ↓
SQLite (Saklama)
    ↓
Form3 (Listeleme/Güncelleme)
    ↓
Form1 (İstatistikler güncelleme)
```

---

## 📝 Örnek Kullanım

### Örnek 1: Yangın Raporu
```
Olay: "Bilgisayar Lab'da kontrollü olmayan elektriksel yangın çıktı"
Kategori (Manual): "Diğer"
[Analiz Et Butonu]
AI Kategorisi: "Yangın"
Aciliyet: "Kritik"
İlk 3 Adım:
1. Alanı tahliye et
2. İtfaiye'yi ara (112)
3. Yangın söndürme cihazı kullan
[Kaydet Butonu]
```

### Örnek 2: Hırsızlık Raporu
```
Olay: "Spor kompleksinden 50 futbol topu çalındı"
Kategori (Manual): "Hırsızlık"
[Analiz Et Butonu]
AI Kategorisi: "Hırsızlık"
Aciliyet: "Orta"
İlk 3 Adım:
1. Güvenlik kamerasını kontrol et
2. Envanteri düzenle
3. Polis raporunu tut
[Kaydet Butonu]
```

---

## 🐛 Sorun Giderme

### Sorun: "API Key boş" uyarısı
**Çözüm:** Form4'ten API Key'i girin ve Kaydet'e tıklayın

### Sorun: "Veritabanı başlatma hatası"
**Çözüm:** SQLite DB Path'in geçerli bir dizin olduğundan emin olun

### Sorun: "Ağ hatası"
**Çözüm:** İnternet bağlantınızı kontrol edin, API servislerinin açık olup olmadığını doğrulayın

### Sorun: Uygulamada "Yükleme Hatası"
**Çözüm:** `%LocalAppData%/KampusGuvenlikAI/application.log` dosyasını kontrol edin

---

## 📞 İletişim ve Destek

- **Geliştirici:** ESMA-nur-CMD
- **Repository:** https://github.com/ESMA-nur-CMD/Kampus_security_AI
- **Lisans:** MIT (İsteğe bağlı)

---

## 📅 Sürüm Tarihi

- **v1.0.0** - 10 Haziran 2026
  - ✅ Hava Durumu Widget'ı
  - ✅ AI Analiz Butonu
  - ✅ Aktif Olaylar Paneli
  - ✅ İstatistik Kartları
  - ✅ Tam Exception Handling

---

## 🎓 Eğitim Amaçlı Notlar

Bu proje şu konuları kapsamaktadır:
- ✅ **API Entegrasyonu** (OpenWeatherMap, OpenAI)
- ✅ **Veritabanı Yönetimi** (SQLite CRUD)
- ✅ **NuGet Paketleri** (Newtonsoft.Json, Microsoft.Data.Sqlite)
- ✅ **Exception Handling** (try-catch-finally)
- ✅ **Async/Await** (Asynchronous programming)
- ✅ **Windows Forms** (UI tasarımı)
- ✅ **JSON Parse** (API yanıtları işleme)
- ✅ **Fallback Mekanizması** (Hata durumlarında alternatif çözüm)

---

**Başarılı bir kampus güvenliği uygulaması için dilerim!** 🎯
