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

<details>
<summary><strong>한국어 설명</strong></summary>

### 🚀 주요 기능
- **시각적 디스크 맵**: 색상과 비율에 맞춰 파티션 구조를 보여주는 동적 시각화 시스템입니다.
- **S.M.A.R.T. 상태 모니터링:**: WMI를 통해 선택한 디스크의 하드웨어 상태 ( 정상/경고)를 실시간으로 확인합니다.
- **미디어 유형 식별:**: 디스크 장치 유형 (SSD, NVMe 또는 HDD)을 자동으로 식별합니다.
- **현대적 파티셔닝 아키텍처**: 복구 파티션을 디스크 끝으로 이동시키는 전문 구조입니다.
  > `[ 부팅 ] ➔ [ Windows ] ➔ [ 데이터 ] ➔ [ 복구 ]`
- **향상된 VHD/X 관리**: 덮어쓰기 방지 및 작업 유효성 검사 로직을 통해 오류 없는 가상 디스크 관리를 제공합니다.
- **정밀한 크기 조정**: 작은 디스크의 경우 10진수 GB 단위로 표시하고 스마트 경계 관리를 지원합니다 (0GB 표시 문제 수정)
- **선택 지속성**: 선택한 디스크는 이제 언어 변경 또는 목록 새로 고침 중에 보존됩니다.
- **가장 최상단 메시지 상자**: WinPE/Live OS 환경에서 모든 경고 팝업에 대한 가시성 보장합니다.
- **상세 확인**: 모든 계획된 작업을 나열하는 여러 줄의 눈에 띄는 경고가 있는 전문 요약 창입니다.
- **견고한 인프라**: WMI 서비스가 없는 제한된 환경(일부 수정된 WinPE)에서도 오류 메시지를 지속적으로 표시하면서 Diskpart 작업을 원활하게 계속할 수 있습니다.
- **다국어 지원**: 한국어, 튀르키예어 또는 영어 인터페이스를 자동으로 감지합니다.

### 📚 사용 시나리오
1. **Windows 클린 설치 (WinPE)**: 설치 미디어에서 부팅한 후, 디스크를 GPT로 준비하고 부팅 가능한 파티션을 몇 초 만에 완벽하게 만듭니다.
2. **가상 디스크 관리**: 테스트용 VHD/VHDX 파일을 생성하고 즉시 첨부하여 물리적 하드웨어인 것처럼 분할합니다.
3. **심리스 파티셔닝**: 클릭 한 번으로 DATA 및 Windows 섹션으로 나누어 새 디스크를 준비합니다.

---

## 🛠 기술적 세부사항
- **프레임워크**: .NET 8 (Windows 양식)
- **백엔드 엔진**: Windows Diskpart.exe
- **요구 사항**: 관리자 권한

---
</details>

<div align="center">
  
Made with ❤️ by [Abdullah ERTÜRK](https://github.com/abdullah-erturk)

<div align="center">

[🌐 erturk.netlify.app](https://erturk.netlify.app)  
</div>
