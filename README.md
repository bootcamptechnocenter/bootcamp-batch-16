## Training Dotnet

Repo ini berisi materi training .NET yang diajarkan oleh **Mas Dimas** dan **Mas Surya**. Materi pengenalan ada di folder `Day1` dan `Day2`, lalu berlanjut ke pembuatan proyek backend `WebApi` dan frontend `WebView`.

## Struktur Singkat

- `Day1`, `Day2`: materi dasar/pengenalan .NET
- `WebApi`: backend API
- `WebView`: frontend MVC yang konsumsi `WebApi`
- `WebApi.Infrastructure`: layer infrastruktur (EF Core, database, konfigurasi entity)
- `WebApi.Modules.Master`: modul master data (service, DTO, dan logic domain untuk master data)
- `WebApi.Shared`: library bersama (shared models, helpers, dan utilities)

Catatan:

- File `appsettings*.json` tidak disimpan di repo. Hubungi pemilik repo untuk mendapatkan konfigurasi yang dibutuhkan.

## Menjalankan Project

Prerequisite:

- .NET SDK (versi sesuai project)
- Database PostgreSQL

### Konfigurasi

Letakkan file di:

- `WebApi/appsettings.json`
- `WebView/appsettings.json`

Copy dari file contoh:

- `WebApi/appsettings.example.json` -> `WebApi/appsettings.json`
- `WebView/appsettings.example.json` -> `WebView/appsettings.json`

### Migrasi Database

Jalankan dari root repo:

```bash
dotnet ef database update --project WebApi.Infrastructure --startup-project WebApi
```

### Menjalankan WebApi

```bash
dotnet run --project WebApi
```

Default URL:

- http://localhost:5168
- https://localhost:7257

### Menjalankan WebView

```bash
dotnet run --project WebView
```

Default URL:

- http://localhost:5242
- https://localhost:7264

Catatan:

- Pastikan `WebApi` sudah running sebelum `WebView`.
- `WebView` mengambil base URL API dari `WebView/appsettings*.json`.
