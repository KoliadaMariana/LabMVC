# 📖 Książka Kucharska — RecipeBookMVC

Projekt zaliczeniowy z przedmiotu tworzenia aplikacji internetowych, realizujący system zarządzania ulubionymi przepisami kulinarnymi (Zadanie 14).

## Funkcjonalności

- **Zarządzanie przepisami:** Dodawanie, wyświetlanie szczegółów oraz usuwanie przepisów[cite: 1].
- **Wyszukiwanie i filtrowanie:** Filtrowanie bazy przepisów po kategoriach oraz wyszukiwanie tekstowe po nazwie[cite: 1].
- **System autoryzacji:** Rejestracja i logowanie użytkowników. Dostęp do dodawania i usuwania przepisów jest ograniczony wyłącznie dla zalogowanych osób[cite: 1].
- **Kontrola dostępu (Własność):** Usunięcie przepisu jest możliwe tylko przez użytkownika, który go utworzył.

---

## 🛠 Technologie

- **Backend:** .NET 8.0 / ASP.NET Core MVC[cite: 1]
- **Baza danych:** Entity Framework Core (SQLite)
- **Autoryzacja:** ASP.NET Core Identity[cite: 1]
- **Frontend:** HTML5, CSS3, Bootstrap 5[cite: 1]

---

## 🗄 Architektura bazy danych

Projekt implementuje relacje pomiędzy trzema modelami[cite: 1]:

1. **Recipe** (Model główny) – przechowuje nazwę, składniki, instrukcje, czas gotowania oraz URL zdjęcia[cite: 1].
2. **Category** – relacja _One-to-Many_ (jeden przepis ma jedną kategorię)[cite: 1].
3. **ApplicationUser** – relacja _One-to-Many_ (przepis jest powiązany z autorem, który go dodał)[cite: 1].

---

## 🚀 Kryteria na ocenę wyższą

Zgodnie z wymaganiami projektu zrealizowano następujące elementy rozszerzone[cite: 1]:

1. **Dodatkowe modele i relacje:** Wprowadzenie powiązanych modeli `Category` oraz `ApplicationUser`[cite: 1].
2. **Zaawansowane filtrowanie:** Implementacja dynamicznego wyszukiwania po nazwie i kategorii[cite: 1].
3. **Zaimplementowanie logiki sesji:** Pełny system logowania i kontroli uprawnień (Identity)[cite: 1].
4. **Ostylowanie interfejsu:** Schludny i spójny wizualnie szablon oparty na Bootstrap 5[cite: 1].

---

## 💻 Instrukcja uruchomienia

1. **Klonowanie repozytorium:**

```bash
   git clone [https://github.com/KoliadaMariana/LabMVC.git](https://github.com/KoliadaMariana/LabMVC.git)
```
