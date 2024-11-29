# Kitap İnceleme Platformu

Bu proje, kitapseverlerin kitapları keşfedebileceği, inceleyebileceği ve değerlendirebileceği bir ASP.NET Core web uygulamasıdır.

## Özellikler

### Ana Sayfa

-   Hero section ile dikkat çekici giriş
-   Öne çıkan kitaplar bölümü
-   Son eklenen kitaplar bölümü
-   Türlere göre kitap listeleme

### Kitap Detayları

-   Detaylı kitap bilgileri
-   Benzer kitap önerileri
-   Görsel önizleme
-   Breadcrumb navigasyon

### Kitap Listesi

-   Sayfalama özelliği
-   Filtreleme ve arama
-   Tür bazlı filtreleme
-   Responsive kart tasarımı

## Teknolojiler

-   Backend: ASP.NET Core 8.0
-   Frontend: Bootstrap 5, JavaScript
-   Veritabanı: MySQL
-   ORM: Entity Framework Core
-   Mimari: MVC (Model-View-Controller)

## Veritabanı Yapısı

### Book Model

```csharp
public class Book
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Kitap adı zorunludur.")]
    [StringLength(100, ErrorMessage = "Kitap adı en fazla 100 karakter olabilir.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Yazar adı zorunludur.")]
    [StringLength(100, ErrorMessage = "Yazar adı en fazla 100 karakter olabilir.")]
    public string Author { get; set; }

    [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Sayfa sayısı zorunludur.")]
    [Range(1, 10000, ErrorMessage = "Sayfa sayısı 1 ile 10000 arasında olmalıdır.")]
    public int PageCount { get; set; }

    [Required(ErrorMessage = "Tür zorunludur.")]
    [StringLength(50, ErrorMessage = "Tür en fazla 50 karakter olabilir.")]
    public string Genre { get; set; }

    public string? ImagePath { get; set; }
    public List<Review> Reviews { get; set; } = new List<Review>();
}
```

### Review Modeli

```csharp
public class Review
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Yorum metni zorunludur.")]
    [StringLength(500, ErrorMessage = "Yorum en fazla 500 karakter olabilir.")]
    public string Comment { get; set; }

    [Required(ErrorMessage = "Puan zorunludur.")]
    [Range(1, 5, ErrorMessage = "Puan 1 ile 5 arasında olmalıdır.")]
    public int Rating { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int BookId { get; set; }
    public Book Book { get; set; }
}
```

## Ekran Görüntüleri

### Ana Sayfa

![Ana Sayfa](/screenshots/home.png)

-   Hero section ile etkileyici giriş
-   Öne çıkan kitaplar
-   Son eklenen kitaplar
-   Türlere göre kitaplar

### Kitap Detay Sayfası

![Kitap Detay](/screenshots/book-detail.png)

-   Hero tasarımı
-   Kitap bilgileri
-   Benzer kitap önerileri

### Kitap Listesi

![Kitap Listesi](/screenshots/book-list.png)

-   Sayfalı kitap listesi
-   Filtreleme özellikleri

## Kurulum

1. Repoyu klonlayın

```bash
git clone https://github.com/onurcansevinc/techcareer-fullstack-mastery-bootcamp.git
```

2. MySQL'i yükleyin ve bir veritabanı oluşturun

-   MySQL Workbench veya phpMyAdmin üzerinden yeni bir veritabanı oluşturun:

```sql
CREATE DATABASE KitapIncelemeDB;
```

3. appsettings.json dosyasını düzenleyin

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=KitapIncelemeDB;User=root;Password=yourpassword;"
    }
}
```

Not: `yourpassword` kısmını kendi MySQL şifrenizle değiştirin.

4. Gerekli NuGet paketlerini yükleyin

```bash
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

5. Migration'ları oluşturun ve veritabanını güncelleyin

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

6. Projeyi çalıştırın

```bash
dotnet run
```

### Olası Hatalar ve Çözümleri

1. MySQL bağlantı hatası alırsanız:

-   MySQL servisinin çalıştığından emin olun
-   Bağlantı dizesindeki kullanıcı adı ve şifrenin doğru olduğunu kontrol edin
-   MySQL'in 3306 portunda çalıştığını kontrol edin

2. Migration hatası alırsanız:

```bash
dotnet ef database drop --force
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

3. SSL hatası alırsanız, bağlantı dizesine şunu ekleyin:

```
;SslMode=none
```

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakınız.

## İletişim

Proje Sahibi - [@onurcansevinc](https://github.com/onurcansevinc)

Proje Linki: [https://github.com/onurcansevinc/techcareer-fullstack-mastery-bootcamp](https://github.com/onurcansevinc/techcareer-fullstack-mastery-bootcamp)
