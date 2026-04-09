<a href="https://buymeacoffee.com/abdullaherturk" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" ></a>

# Diskpart GUI v3 - Professional Partitioning Tool

[![.NET 8](https://img.shields.io/badge/.NET-8-blueviolet.svg?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/Language-C%23-blue.svg?style=flat-square&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![WinPE Ready](https://img.shields.io/badge/WinPE-Optimized-orange.svg?style=flat-square)](https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/winpe-intro)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg?style=flat-square)](https://opensource.org/licenses/MIT)
[![Diskpart](https://img.shields.io/badge/Engine-Diskpart-lightgrey.svg?style=flat-square)](https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/diskpart)

![sample](https://github.com/abdullah-erturk/Diskpart-GUI/blob/main/preview.jpg)

## Link:

[![Stable?](https://img.shields.io/badge/Release-v3-green.svg?style=flat)](https://github.com/abdullah-erturk/Diskpart-GUI/releases)

### Nedir?
Diskpart GUI v3, hem **WinPE** (Windows Preinstallation Environment) hem de **Canlı Windows** (Live OS) ortamları için optimize edilmiş, .NET 8 tabanlı profesyonel bir disk bölümlendirme ve formatlama aracıdır.

### What is it?
Diskpart GUI v3 is a professional disk partitioning and formatting tool based on .NET 8, optimized for both **WinPE** (Windows Preinstallation Environment) and **Live Windows** (Live OS) environments.

---

<details>
<summary><strong>Türkçe Tanıtım</strong></summary>

### 🚀 Temel Özellikler
- **Görsel Disk Haritası**: Disk bölümlendirme yapısını renkli ve orantılı bir grafik üzerinde gösteren dinamik görselleştirme sistemi.
- **S.M.A.R.T. Sağlık Takibi**: Seçili diskin donanımsal sağlık durumunun (OK/Warning) WMI üzerinden anlık kontrolü.
- **Medya Tipi Tespiti**: Diskin SSD, NVMe veya HDD olup olmadığını otomatik belirleme zekası.
- **Modern Bölümleme Mimarisi**: Kurtarma (Recovery) bölümünü diskin en sonuna taşıyan profesyonel yapı.
  > `[ Boot ] ➔ [ Windows ] ➔ [ DATA ] ➔ [ Recovery ]`
- **Gelişmiş VHD/VHDX Desteği**: Dosya üzerine yazma koruması, işlem doğrulama zekası ve otomatik bağlama.
- **Hassas Boyutlandırma**: Küçük diskler için ondalıklı GB gösterimi ve akıllı sınır yönetimi (0 GB hatası giderildi).
- **Seçim Koruması**: Dil değişikliği veya liste yenilemede seçili diskin hafızada tutulması.
- **TopMost Mesaj Kutuları**: Tüm uyarı pencerelerinin WinPE ortamlarında her zaman en üstte kalma garantisi.
- **Detaylı Onay Ekranı**: Format işleminden önce çok satırlı ve belirgin uyarılarla listelenen profesyonel özet penceresi.
- **Dayanıklı Altyapı**: WMI servisinin eksik olduğu kısıtlı ortamlarda (bazı modifiye WinPE) bilgilendirici ve kalıcı hata mesajları ile Diskpart işlemlerine kesintisiz devam edebilme gücü.
- **Çoklu Dil Desteği**: İşletim sistemi diline göre otomatik Türkçe veya İngilizce arayüz.

 
### 📚 Kullanım Senaryoları
1. **Temiz Windows Kurulumu (WinPE)**: Kurulum medyasından ön yükleme yaptıktan sonra diski GPT olarak hazırlayıp, UEFI/BIOS bölümlerini saniyeler içinde hatasız oluşturabilirsiniz.
2. **Sanal Disk Yönetimi**: Test amaçlı VHD/VHDX dosyaları oluşturup anında sisteme bağlayabilir, diski fiziksel bir diskmiş gibi yönetebilirsiniz.
3. **Veri Diski Hazırlama**: Yeni aldığınız bir diski tek tıkla DATA ve Windows bölümlerine senkronize bir şekilde ayırabilirsiniz.

## 🛠 Teknik Detaylar
- **Framework**: .NET 8 (Windows Forms)
- **Backend Engine**: Windows Diskpart.exe
- **Gereksinimler**: Yönetici Hakları
---

</details>

<details>
<summary><strong>English Description</strong></summary>

### 🚀 Key Features
- **Visual Disk Map**: Dynamic visualization system showing the partition structure on a colored, proportional graphic.
- **S.M.A.R.T. Health Monitoring**: Real-time checking of the selected disk's hardware health status (OK/Warning) via WMI.
- **Media Type Identification**: Automatic identification of disk device types (SSD, NVMe, or HDD).
- **Modern Partitioning Architecture**: Professional structure that moves the Recovery partition to the end of the disk.
  > `[ Boot ] ➔ [ Windows ] ➔ [ DATA ] ➔ [ Recovery ]`
- **Enhanced VHD/VHDX Support**: Overwrite protection, operation validation logic, and automatic mounting.
- **Precise Sizing**: Decimal GB display for small disks and smart boundary management (Fixed 0 GB display issue).
- **Selection Persistence**: The selected disk is now preserved during language changes or list refreshes.
- **TopMost Message Boxes**: Guaranteed visibility for all warning popups in WinPE/Live OS environments.
- **Detailed Confirmation**: A professional summary window with multi-line, prominent warnings listing all planned operations.
- **Robust Infrastructure**: Ability to continue Diskpart operations seamlessly with informative, persistent error messages in restricted environments (some modified WinPE) where WMI services are missing.
- **Multi-Language Support**: Automatic Turkish or English interface detection.

### 📚 Usage Scenarios
1. **Clean Windows Installation (WinPE)**: After booting from installation media, prepare your disk as GPT and create bootable partitions flawlessly in seconds.
2. **Virtual Disk Management**: Create VHD/VHDX files for testing and attach them instantly, partitioning them as if they were physical hardware.
3. **Seamless Partitioning**: Prepare a new disk by splitting it into DATA and Windows sections with a single click.

---

## 🛠 Technical Details
- **Framework**: .NET 8 (Windows Forms)
- **Backend Engine**: Windows Diskpart.exe
- **Requirements**: Administrator Privileges

---
</details>

<div align="center">
  
Made with ❤️ by [Abdullah ERTÜRK](https://github.com/abdullah-erturk)

<div align="center">

[🌐 erturk.netlify.app](https://erturk.netlify.app)  
</div>
