<a href="https://buymeacoffee.com/abdullaherturk" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" ></a>

# Diskpart GUI - Professional Partitioning Tool

[![.NET 8](https://img.shields.io/badge/.NET-8-blueviolet.svg?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/Language-C%23-blue.svg?style=flat-square&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![WinPE Ready](https://img.shields.io/badge/WinPE-Optimized-orange.svg?style=flat-square)](https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/winpe-intro)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg?style=flat-square)](https://opensource.org/licenses/MIT)
[![Diskpart](https://img.shields.io/badge/Engine-Diskpart-lightgrey.svg?style=flat-square)](https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/diskpart)

![sample](https://github.com/abdullah-erturk/Diskpart-GUI/blob/main/preview.jpg)

## Link:

[![Stable?](https://img.shields.io/badge/Release-v1.svg?style=flat)](https://github.com/abdullah-erturk/Diskpart-GUI/releases)

### Nedir?
Diskpart GUI, hem **WinPE** (Windows Preinstallation Environment) hem de **Canlı Windows** (Live OS) ortamları için optimize edilmiş, .NET 8 tabanlı profesyonel bir disk bölümlendirme ve formatlama aracıdır.

### What is it?
Diskpart GUI is a professional disk partitioning and formatting tool based on .NET 8, optimized for both **WinPE** (Windows Preinstallation Environment) and **Live Windows** (Live OS) environments.

---

<details>
<summary><strong>Türkçe Tanıtım</strong></summary>

### 🚀 Temel Özellikler
- **Windows Yüklü Diski Koruma**: OS yüklü olan disk bölümünün çalışan Windows ortamında formatlanması önlenir.
- **GPT ve MBR Desteği**: Modern (UEFI-GPT) ve eski (BIOS-MBR) sistemler için tam uyumluluk.
- **Akıllı Boyut Dengeleme**: Girilen bölüm boyutları disk kapasitesini aşarsa, Windows bölümünü otomatik olarak küçülterek çakışmaları önler.
- **VHD/VHDX Desteği**: Saniyeler içinde dinamik boyutlu sanal disk oluşturma ve otomatik bağlama.
- **WinPE Optimizasyonu**: Formatlanan OS bölümüne istisnasız **C:** harfini atama zekası (C: başka bir diskte rezerve değilse).
- **Detaylı Onay Ekranı**: Format işleminden önce yapılacak tüm işlemleri (Boot, OS, Data, Recovery) listeleyen profesyonel özet penceresi.
- **Çoklu Dil Desteği**: İşletim sistemi diline göre otomatik Türkçe veya İngilizce arayüz. INI dosyalarının çevrilmesi ile farklı dil desteği eklenebilir.

 
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
- **Protecting the Disk Where Windows is Installed**: Prevents the disk partition where the OS is installed from being formatted in the running Windows environment.
- **GPT & MBR Support**: Full compatibility for both modern (UEFI-GPT) and legacy (BIOS-MBR) systems.
- **Smart Size Balancing**: Automatically shrinks the Windows partition if the total size exceeds capacity, preventing formatting errors.
- **VHD/VHDX Support**: Create and automatically mount a dynamically sized virtual disk in seconds.
- **WinPE Optimization**: Smart drive letter assignment that prioritizes **C:** for the OS partition.
- **Detailed Confirmation**: A professional summary window listing all planned operations (Boot, OS, Data, Recovery) before formatting.
- **Multi-Language Support**: Automatic Turkish or English interface detection. Support for different languages ​​can be added by translating the INI files.

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
