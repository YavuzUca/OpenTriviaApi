# OpenTriviaApi
## Prerequisites
### Required
- .NET 10 SDK (for building and running locally)
- Git (to clone the repository)

### Verify installation:
```bash
dotnet --version
```
The version should start with 10.

```bash
git --version
```

### Running the application

Open a terminal and navigate to the Trivia folder:

```bash
cd Trivia
```

Restore dependencies:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```

Run the application using the HTTP launch profile:

```bash
dotnet run --launch-profile "http"
```

#### Notes

The backend API must be available on port `5172`. Ensure that nothing else is using this port before starting the application.

### Running unit tests

To run all unit tests, first go to the directory Trivia.Tests:
```bash
cd Trivia.Tests
```

Here you can run the tests.
```bash
dotnet test
```


This will build the test projects and execute all configured unit tests.
