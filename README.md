# BKM Hacker News Viewer

A simple application to view the newest Hacker News stories, built with an Angular frontend and a C# .NET Core backend API.

## Features
- Displays a paginated list of the newest stories from Hacker News.
- Each story includes a title and a link to the article (falls back to the HN item page if no URL is provided).
- Search functionality to filter stories by title.
- Paging with 20 stories per page.
- Backend caches stories for 1 minute to reduce API calls.
- Automated tests for both backend and frontend.

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/) (includes npm)
- [Angular CLI 17+](https://angular.dev/) (install globally via `npm install -g @angular/cli`)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) (recommended for easier HTTPS setup)

## Setup and Build
1. Clone the repository:
   ```
   git clone <your-repo-url>
   cd HackerNewsViewer
   ```

2. **Backend Setup**:
   - Navigate to the backend directory: `cd HackerNewsViewer`
   - Restore dependencies: `dotnet restore`
   - Build the project: `dotnet build`

3. **Frontend Setup**:
   - Navigate to the frontend directory: `cd hn-angular`
   - Install dependencies: `npm install`

## Running the Application
The backend API uses HTTPS in development for security. If using the CLI (`dotnet run`), ensure the .NET development certificate is installed and trusted (see below). Alternatively, open the solution in Visual Studio for automatic HTTPS handling.

### Option 1: Run via Visual Studio (Recommended for HTTPS)
1. Open the solution file (`HackerNewsViewer.sln`) in Visual Studio.
2. Set `HackerNewsViewer` as the startup project (right-click > Set as Startup Project).
3. Press F5 or click Run (IIS Express) to start the backend with HTTPS.
   - The API will launch on `https://localhost:<port>` (check Output window or browser launch).
   - Swagger: `https://localhost:<port>/swagger/index.html`

4. In a separate terminal, run the frontend from `hn-angular`: `ng serve --proxy-config proxy.conf.json`
5. Open `http://localhost:4200` in your browser.

### Option 2: Run via CLI (dotnet run)
1. Install and trust the .NET dev certificate (required for HTTPS):
   ```
   dotnet dev-certs https --trust
   ```
   - On Windows/macOS, this installs a self-signed cert. Follow any prompts to trust it.
   - If issues arise, see [Microsoft docs on enforcing HTTPS](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl).

2. From the `HackerNewsViewer` directory: `dotnet run`
   - The API should start on `https://localhost:7009` (or check console for URLs).
   - If it falls back to HTTP, verify the cert with `dotnet dev-certs https --check` and ensure `launchSettings.json` has an HTTPS profile.
   - Test via Swagger: `https://localhost:7009/swagger/index.html`
   - Example endpoint: `https://localhost:7009/api/stories?page=1&pageSize=20`

3. In a separate terminal, run the frontend from `hn-angular`: `ng serve --proxy-config proxy.conf.json`
4. Open `http://localhost:4200` in your browser.

**Note on HTTPS Issues**: If `dotnet run` only runs on HTTP, double-check the dev cert installation. Visual Studio handles this automatically via IIS Express.

## Running Tests
1. **Backend Tests**:
   - From the solution HackerNewsViewer.Tests directory: `dotnet test`
   - Or in Visual Studio: Test > Test Explorer > Run All.
   - Includes unit tests for services/controllers and integration tests.

2. **Frontend Tests**:
   - From the `hn-angular` directory: `ng test`
   - Includes unit tests for components and services (runs in watch mode by default).

## Notes
- The backend fetches up to 200 newest stories from the Hacker News API and caches them in memory.
- Search and paging are handled server-side for efficiency.
- If you encounter proxy issues, verify the `proxy.conf.json` targets the correct backend URL (e.g., `"target": "https://localhost:7009"`).
- For production deployment, consider hosting the Angular build statically and the API on a server (e.g., via `dotnet publish` and IIS/Azure).

If you have issues, check console logs for errors or ensure ports are not conflicting.