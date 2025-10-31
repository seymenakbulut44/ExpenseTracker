# ExpenseTracker - Gider Takip Uygulaması

Modern ve kullanıcı dostu bir gider takip uygulaması. Gelir ve giderlerinizi kategorilere göre organize edin, finansal durumunuzu görsel grafiklerle takip edin.

## 📋 İçindekiler

- [Özellikler](#özellikler)
- [Teknolojiler](#teknolojiler)
- [Kurulum](#kurulum)
- [Kullanım](#kullanım)
- [Proje Yapısı](#proje-yapısı)
- [Güvenlik](#güvenlik)
- [Katkıda Bulunma](#katkıda-bulunma)

## ✨ Özellikler

- **Kullanıcı Kimlik Doğrulama**: ASP.NET Core Identity ile güvenli kullanıcı yönetimi
- **Kategori Yönetimi**: Gelir ve giderlerinizi kategorilere göre organize edin
- **İşlem Yönetimi**: Gelir ve gider ekleme, düzenleme ve silme işlemleri
- **Dashboard**: Toplam gelir, gider ve bakiye görüntüleme
- **Görsel Grafikler**: Kategori bazlı gider dağılımını Chart.js ile görselleştirme
- **Çoklu Kullanıcı Desteği**: Her kullanıcı kendi verilerini yönetir
- **Responsive Tasarım**: Mobil ve masaüstü cihazlarda uyumlu çalışır

## 🛠️ Teknolojiler

### Backend
- **ASP.NET Core 8.0**: Modern web framework
- **Entity Framework Core 8.0**: ORM (Object-Relational Mapping) aracı
- **SQL Server**: İlişkisel veritabanı
- **ASP.NET Core Identity**: Kullanıcı kimlik doğrulama ve yetkilendirme sistemi
- **Razor Pages**: Sunucu tarafı HTML rendering

### Frontend
- **Bootstrap**: Responsive CSS framework
- **Chart.js 4.4.1**: İnteraktif grafik kütüphanesi
- **jQuery**: JavaScript kütüphanesi
- **jQuery Validation**: Form doğrulama eklentileri
- **jQuery Validation Unobtrusive**: ASP.NET Core ile entegre form doğrulama

### NuGet Paketleri
- `Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore` (8.0.18)
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (8.0.18)
- `Microsoft.AspNetCore.Identity.UI` (8.0.18)
- `Microsoft.EntityFrameworkCore.SqlServer` (8.0.18)
- `Microsoft.EntityFrameworkCore.Tools` (8.0.18)

## 🚀 Kurulum

### Gereksinimler

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) veya üzeri
- SQL Server (LocalDB, SQL Server Express veya SQL Server)
- Visual Studio 2022 veya Visual Studio Code (opsiyonel)

### Adımlar

1. **Projeyi klonlayın**
   ```bash
   git clone https://github.com/kullaniciadi/ExpenseTracker.git
   cd ExpenseTracker
   ```

2. **Veritabanı bağlantı string'ini yapılandırın**
   
   `appsettings.json` dosyasında `ConnectionStrings` bölümünü kendi SQL Server bağlantı bilgilerinize göre güncelleyin:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=SUNUCU_ADI;Database=ExpenseTrackerDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Veritabanı migration'larını çalıştırın**
   ```bash
   dotnet ef database update
   ```

   Eğer Entity Framework Core Tools yüklü değilse:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

4. **Uygulamayı çalıştırın**
   ```bash
   dotnet run
   ```

5. **Tarayıcıda açın**
   
   Uygulama genellikle `https://localhost:5001` veya `http://localhost:5000` adresinde çalışır.

## 📱 Kullanım

### İlk Kullanım

1. **Hesap Oluşturma**: Uygulama ilk açıldığında "Register" linkine tıklayarak yeni bir hesap oluşturun.
2. **Giriş Yapma**: Hesap oluşturduktan sonra otomatik olarak giriş yaparsınız.
3. **İlk Kategoriler**: İlk girişinizde otomatik olarak bazı varsayılan kategoriler oluşturulur:
   - Food (Yiyecek)
   - Rent (Kira)
   - Transport (Ulaşım)
   - Utilities (Faturalar)
   - Salary (Maaş)

### Kategori Yönetimi

- **Kategori Ekleme**: "Categories" menüsünden yeni kategoriler ekleyebilirsiniz.
- **Kategori Düzenleme**: Mevcut kategorileri düzenleyebilirsiniz.
- **Kategori Silme**: Artık kullanmadığınız kategorileri silebilirsiniz.

**Not**: Her kullanıcı için kategori isimleri benzersiz olmalıdır.

### İşlem Yönetimi

- **İşlem Ekleme**: "Transactions" menüsünden gelir veya gider ekleyebilirsiniz.
- **İşlem Tipi**: Her işlem "Income" (Gelir) veya "Expense" (Gider) olarak işaretlenebilir.
- **İşlem Bilgileri**: Her işlem için tutar, tarih, kategori ve notlar ekleyebilirsiniz.

### Dashboard

Ana sayfa (Dashboard) üzerinden:
- Toplam gelirlerinizi görüntüleyebilirsiniz
- Toplam giderlerinizi görüntüleyebilirsiniz
- Bakiyenizi takip edebilirsiniz
- Kategori bazlı gider dağılımını grafik olarak görebilirsiniz

## 📁 Proje Yapısı

```
ExpenseTracker/
├── Controllers/          # MVC Controller'ları
│   ├── CategoriesController.cs
│   ├── HomeController.cs
│   └── TransactionsController.cs
├── Data/                # Veritabanı context ve migration'lar
│   ├── ApplicationDbContext.cs
│   └── Migrations/
├── Models/              # Veri modelleri
│   ├── Category.cs
│   ├── Transaction.cs
│   └── ErrorViewModel.cs
├── Views/               # Razor view dosyaları
│   ├── Categories/
│   ├── Home/
│   ├── Transactions/
│   └── Shared/
├── Pages/               # Razor Pages
│   └── Areas/Identity/
├── wwwroot/             # Statik dosyalar (CSS, JS, resimler)
│   ├── css/
│   ├── js/
│   └── lib/             # Kütüphaneler (Bootstrap, jQuery, Chart.js)
├── Program.cs           # Uygulama giriş noktası ve yapılandırma
└── appsettings.json     # Uygulama ayarları
```

## 🔒 Güvenlik

- **Kimlik Doğrulama**: Tüm işlemler için kullanıcı kimlik doğrulaması zorunludur (`[Authorize]` attribute)
- **Kullanıcı İzolasyonu**: Her kullanıcı sadece kendi verilerine erişebilir
- **Anti-Forgery Token**: Tüm form işlemlerinde CSRF koruması aktif
- **HTTPS**: Üretim ortamında HTTPS yönlendirmesi etkin
- **SQL Injection Koruması**: Entity Framework Core ile parametreli sorgular kullanılıyor

## 🎨 Özelleştirme

### Varsayılan Kategoriler

`HomeController.cs` dosyasındaki `SeedDefaultCategoriesAsync` metodunu düzenleyerek ilk oluşturulan kategorileri değiştirebilirsiniz.

### Stil Değişiklikleri

`wwwroot/css/site.css` dosyasını düzenleyerek görsel özelleştirmeler yapabilirsiniz.

## 🤝 Katkıda Bulunma

Katkılarınızı memnuniyetle karşılıyoruz! Lütfen şu adımları izleyin:

1. Bu repository'yi fork edin
2. Yeni bir feature branch oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -am 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/YeniOzellik`)
5. Bir Pull Request oluşturun

## 📝 Lisans

Bu proje açık kaynaklıdır ve serbestçe kullanılabilir.

## 👤 Yazar

Bu proje geliştirilmekte olan bir gider takip uygulamasıdır.

---

**Not**: Bu uygulama eğitim ve kişisel kullanım amaçlı geliştirilmiştir. Üretim ortamında kullanmadan önce güvenlik ve performans testlerini yapmayı unutmayın.
