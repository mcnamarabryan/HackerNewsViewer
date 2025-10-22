# HN Viewer

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

- 
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
1. **Run the Backend**:
   - From the `HackerNewsViewer` directory: `dotnet run`
   - The API will start on `https://localhost:7009` (confirm in console output).
   - Test the API via Swagger: `https://localhost:7009/swagger/index.html`
   - Example endpoint: `https://localhost:7009/api/stories?page=1&pageSize=20`

2. **Run the Frontend**:
   - From the `hn-angular` directory: `ng serve --proxy-config proxy.conf.json`
   - The app will start on `http://localhost:4200`.
   - Open `http://localhost:4200` in your browser to view the app.
   - The proxy config forwards `/api` calls to the backend (ensure backend is running).

## Running Tests
1. **Backend Tests**:
   - From the `HackerNewsViewer` directory: `dotnet test`
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