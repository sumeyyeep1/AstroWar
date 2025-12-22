# 🚀 Astro War - 2D Space Shooter

> Kırklareli Üniversitesi Oyun Programlama dersi kapsamında geliştirilmiş, Unity tabanlı sonsuz uzay savaşı oyunu.

![Unity](https://img.shields.io/badge/Engine-Unity%206-black)
![C#](https://img.shields.io/badge/Language-C%23-blue)
![Platform](https://img.shields.io/badge/Platform-WebGL%20%7C%20PC-green)
![Status](https://img.shields.io/badge/Status-Completed-success)

## 🎮 Oyun Hakkında

**Astro War**, oyuncuların uzayın derinliklerinde düşman gemileri ve asteroitler arasında hayatta kalmaya çalıştığı, **Retro Arcade** tarzında bir 2D "Endless Shooter" oyunudur.

Oyunun temel amacı; sonsuz bir döngüde ilerlerken düşmanları yok etmek, can iksirlerini toplamak ve giderek zorlaşan seviyelerde en yüksek skoru elde etmektir.

**🔗 Oynanabilir Link (Itch.io):** [https://smeyyep1.itch.io/astro]

## ✨ Özellikler

* **Sonsuz Oynanış:** Oyuncu hayatta kaldığı sürece oyun devam eder.
* **Dinamik Zorluk Sistemi:** Skor arttıkça (Level atladıkça) düşmanların hızı ve saldırı sıklığı artar.
* **Akıllı Düşmanlar:** Düşman gemileri sadece aşağı inmez, oyuncuya ateş ederek karşılık verir.
* **Can ve Güçlendirme:** Rastgele düşen "Can İksirleri" ile oyuncu hasar aldığında iyileşebilir.
* **Pixel Art Tasarım:** 8-bit nostaljik grafikler ve uzay atmosferi.
## 📸 Oynanış Görüntüleri (Screenshots)
<img width="989" height="703" alt="Ekran Görüntüsü (88)" src="https://github.com/user-attachments/assets/c6d27e5a-fead-4219-b21c-a30e2a8fc71d" />
<img width="967" height="628" alt="Ekran Görüntüsü (89)" src="https://github.com/user-attachments/assets/306a2421-39e5-49fa-9bd5-a3edf65ce430" />
<img width="962" height="583" alt="Ekran Görüntüsü (90)" src="https://github.com/user-attachments/assets/2aab3b4d-d8bb-4934-8162-653081ef138e" />
<img width="960" height="613" alt="Ekran Görüntüsü (87)" src="https://github.com/user-attachments/assets/b389d72c-a155-4b85-a124-06c6327f7d62" />

## 🕹️ Kontroller

| Tuş | İşlev |
| :--- | :--- |
| **Yön Tuşları / WASD** | Uzay gemisini hareket ettirir. |
| **Boşluk (Space)** | Ateş eder (Lazer). |
| **ESC** | Oyunu durdurur (Pause Menu). |

## 🛠️ Teknik Detaylar ve Scriptler

Oyun mimarisi **Object-Oriented Programming (OOP)** prensiplerine uygun olarak tasarlanmıştır. Öne çıkan scriptler:

### 1. Game Manager (Oyun Yöneticisi)
Oyunun beynidir. Skor takibi, can yönetimi, level artışı ve sahne geçişlerini (Game Over / Restart) yönetir. Singleton tasarım deseni mantığıyla çalışır.

### 2. Spawner (Üretici)
"Object Pooling" mantığına benzer şekilde, belirlenen süre aralıklarında (`InvokeRepeating`) ekranın üst kısmında rastgele X koordinatlarında düşman ve can iksiri üretir.

### 3. Player Controller
Oyuncunun sınırlandırılmış hareketini (`Mathf.Clamp`) ve ateş etme mekaniğini (`Instantiate`) kontrol eder.

### 4. Düşman Yapay Zekası
Düşmanlar, oyuncunun seviyesine göre hızlarını ayarlar. Hem hareket eder hem de belirli aralıklarla oyuncuya mermi fırlatır.

## 📂 Proje Kurulumu

Projeyi Unity editöründe açmak için:

1.  Bu depoyu klonlayın:
    ```bash
    git clone [https://github.com/sumeyyeep1/AstroWar.git](https://github.com/sumeyyeep1/AstroWar.git)
    ```
2.  **Unity Hub**'ı açın ve `Add Project` diyerek klasörü seçin.
3.  Unity (Sürüm 6 veya üstü önerilir) ile projeyi başlatın.
4.  `Scenes/MainMenu` sahnesini açarak oyunu test edebilirsiniz.

## 👥 Geliştirici Ekip

* **Sümeyye Polat** - 1220505058
* **Merve Mızraklı** - 1220505052

---
*Kırklareli Üniversitesi Yazılım Mühendisliği - Oyun Programlama Dersi Final Projesidir.*
