# Smakolyk – Kolekcja Ulubionych Przepisów Kulinarnych

## Spis treści

1. Opis projektu
2. Funkcjonalności
3. Struktura aplikacji MVC
4. Technologie
5. Modele danych
6. Uruchomienie projektu
7. Autor

---

# 1. Opis projektu

Smakolyk to aplikacja internetowa stworzona w technologii ASP.NET Core MVC, której celem jest zarządzanie przepisami kulinarnymi.

Użytkownicy mogą rejestrować konta, logować się do systemu, dodawać własne przepisy, edytować je, usuwać oraz przeglądać przepisy innych użytkowników. Aplikacja umożliwia również ocenianie przepisów, wyszukiwanie i filtrowanie danych oraz korzystanie z funkcji Premium.

Projekt został wykonany zgodnie ze wzorcem architektonicznym MVC (Model-View-Controller).

---

# 2. Funkcjonalności

## Zarządzanie przepisami

- dodawanie przepisów,
- edycja własnych przepisów,
- usuwanie własnych przepisów,
- wyświetlanie szczegółów przepisu.

## Wyszukiwanie i filtrowanie

- wyszukiwanie po nazwie przepisu,
- filtrowanie po kategorii,
- filtrowanie po czasie przygotowania.

## System użytkowników

- rejestracja użytkowników,
- logowanie,
- wylogowanie,
- autoryzacja dostępu do wybranych funkcji.

## Opinie i oceny

- dodawanie ocen do przepisów,
- przechowywanie opinii użytkowników,
- obliczanie średniej oceny.

## Profil użytkownika

- wyświetlanie liczby dodanych przepisów,
- wyświetlanie średniej oceny wszystkich przepisów użytkownika,
- lista własnych przepisów.

## Premium

- aktywacja konta Premium,
- specjalny status użytkownika,
- dostęp do funkcji „Moja Lodówka”.

## Inteligentna Lodówka

- wybór dostępnych składników,
- wyszukiwanie przepisów na podstawie posiadanych produktów,
- filtrowanie przepisów według zawartości lodówki.

---

# 3. Struktura aplikacji MVC

## Models

- Recipe
- Category
- Review
- ApplicationUser

## Views

Widoki odpowiedzialne za prezentację danych użytkownikowi:

- Home
- Recipes
- Profile
- Premium
- Fridge
- Account

## Controllers

- HomeController
- RecipesController
- ReviewsController
- PremiumController
- FridgeController
- ProfileController
- AccountController

---

# 4. Technologie

Projekt został wykonany z wykorzystaniem:

- ASP.NET Core MVC
- C#
- Entity Framework Core
- ASP.NET Identity
- SQLite
- Bootstrap 5
- Razor Views

---

# 5. Modele danych

## Recipe

Przechowuje informacje o przepisie:

- nazwa,
- składniki,
- instrukcja przygotowania,
- czas przygotowania,
- poziom trudności,
- zdjęcie,
- autor przepisu,
- kategoria.

## Category

Przechowuje kategorię przepisu.

## Review

Przechowuje ocenę oraz opinię użytkownika.

## ApplicationUser

Rozszerzony model użytkownika zawierający:

- imię,
- nazwisko,
- dane logowania,
- status Premium.

---

# 6. Uruchomienie projektu

## Wymagania

- .NET 8 lub nowszy
- Visual Studio / Rider
- SQLite

## Instalacja

1. Sklonować repozytorium:

```bash
git clone https://github.com/nazwa-repozytorium.git
```

2. Otworzyć projekt w Visual Studio lub Rider.

3. Przywrócić pakiety NuGet.

4. Wykonać migracje:

```bash
Update-Database
```

5. Uruchomić aplikację.

---

# 7. Autor

Mariana Koliada

Projekt zaliczeniowy wykonany w ramach przedmiotu:

**Wzorzec MVC w tworzeniu aplikacji internetowych**
