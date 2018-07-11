# Teknik-Servis
Karabük Üniversitesi Bilgisayar Mühendisliği Bölümü "Senior Project" dersi için oluşturuldu.

Projede çoklu teknik servis otomasyonu oluşturulmuştur(Kvk, Telpa vb. gibi). Bu senaryoya göre müşteri lokasyonuna yakın teknik servis noktasını belirledikten sonra arıza kayıtıdını oluşturur.
Cihazın kayıt olunduğu servis yöneticileri, cihaz garanti kapsamını belirleme, garanti dışı olan cihazlar için ücret bilgisi girilmesi, garanti kapsamında olan yada ücreti ödenmiş cihazların servis personeline atanması, iptal ya da tamamlanmış ürünleri kargoya çıkarılması gibi işlemlerden sorumludur.
Yönetici izni olan servis sekreteri belirli aralıkları hesapları kontrol ederek cihaz ücret ödenme durumlarını günceller ve kargo durumlarını günceller.
Servis Personeli yönetici tarafından kendine atanan cihazların onarımını yapar ve stok güncellemesi yapar.(Servis yöneticisi de bu izne sahiptir.)
Servis Personeli ve Yöneticisi cihazlar hakkında raporlar tutabilir; Personelin tuttuğu raporlar yönetici tarafından görülür fakat son kullanıcı tarafından görülemez.Cihaz hakkında ki yönetici raporları ise son kullanıcı(müşteri) tarafından izlenebilir.
Son Kullanıcı( Arızalı cihaz kayıtı olan müşteri) cihaz takibini kayıt sonrası gönderilen form numarası  ya da cihaz İmei No ilegerçekleştirir. Cihaz için ücret talebinde bulunması durumunda bu sorgu sayfası üzerinden hizmet iptal talebinde bulunabilir.Kargo ve ücret bilgilerini bu sayfadan görüntüleyebileceği gibi sistem tarafından kayıtlı mail adresine de gönderim yapılır.

Bu proje Asp.Net MVC mimarisi ile tasarlanmıştır. Entity Framework Kullanılmış ve veritabanı tarafında EF' nin Code First yapısı kullanılmıştır. Proje de SOLID prensipleri gözetilmiştir. Web harici arayüzlere uygulanabilirliği için ek katmanlar eklenmiş ve soyutlamalar yapılmıştır.

sunum-afiş klasörü içerisinde iş akış şeması, yerelde çalışmış hali ve kullanılan teknolojilere ait resimler bulunmaktadır.
