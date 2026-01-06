# 🥗 HealthBite

HealthBite is a cross-platform health and nutrition tracking application built with **.NET MAUI** following the **MVVM architecture**.  
The goal of the project is to provide users with a structured way to track body metrics, meals, nutrition habits and generate meaningful insights for healthier lifestyle planning.

---

## 🚀 Features (Planned / Implemented)

- Cross-platform UI (Android / iOS / Windows)
- MVVM-based modular architecture
- Page navigation using **.NET MAUI Shell**
- Structured separation of:
  - Models
  - Views
  - ViewModels
  - Services
  - Data resources
- Extensible service layer for:
  - Local storage
  - Future API / backend integration
- Reusable UI components and global styling via XAML resources

> ⚠️ Backend (ASP.NET Core API) is **not included yet** — the app currently focuses on the client-side architecture.  
> The project is designed so that API integration can be added easily later.

---

## 🧩 Project Structure

```text
HealthBite
├── Data/          # Static / seed data helpers
├── Models/        # Domain models (User, Body metrics, Meals, etc.)
├── Services/      # Business & data access layer
├── ViewModels/    # Page logic, bindings, commands (MVVM)
├── Views/         # XAML pages (UI)
├── Resources/     # Styles, fonts, images
├── Platforms/     # Platform-specific code (Android / iOS / Windows)
├── App.xaml       # Global resources & theme
├── AppShell.cs    # Shell navigation configuration
└── MauiProgram.cs # Dependency Injection & app bootstrap
```

(Database & API integration will be added in future iterations.)

---

## 🔮 Planned Enhancements

- ASP.NET Core Web API backend
- User profile & authentication
- Nutrition recommendation engine
- SQLite local database
- Analytics & progress dashboards
- AI-assisted body-composition insights

---

## 📝 License

This project is currently under development and not yet licensed.

