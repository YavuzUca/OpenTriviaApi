# OpenTriviaApi
This was a simulated project in which the goal was to build a simple trivia web application. The Open Trivia Database API was used to generate questions, but it exposes the correct answers in its JSON response, making it possible for users to inspect and cheat.
To solve this, an intermediate back-end (Java or C#) was developed with two endpoints: GET /questions to retrieve questions securely and POST /checkanswers to validate answers on the server side.
A simple user interface was also created to allow users to answer questions and receive feedback.

## Prerequisites
This is the backend of my project. You also need my frontend project called [OpenTriviaUI](https://github.com/YavuzUca/OpenTriviaUI).

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

## Cloning the repository
Pick a desired location on your machine and run this command:

```bash
git clone https://github.com/YavuzUca/OpenTriviaApi.git
```

## Running the application

After cloning the repository, there will be two folders called 'Trivia' and 'Trivia.Tests'. Inside the OpenTriviaApi folder, go to the Trivia folder:

```bash
cd .\Trivia\
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

The back-end will now run on localhost:5172. You can also try out the API's on the Swagger page without needing the UI. In order to do so, just configure the Start-up Project in Visual Studio to start the Trivia project. Do note that you the Swagger start option uses a different port than the normal start.

#### Notes

The backend API must be available on port `5172`. Ensure that nothing else is using this port before starting the application.

## Running unit tests

To run all unit tests, first go to the directory Trivia.Tests:
```bash
cd .\Trivia.Tests\
```

Here you can run the tests.
```bash
dotnet test
```


This will build the test projects and execute all configured unit tests.
