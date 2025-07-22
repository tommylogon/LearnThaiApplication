# Learn Thai Application

This is a Windows Presentation Foundation (WPF) application designed to help users learn the Thai language. It provides a user-friendly interface for learning Thai script, phonetics, and vocabulary. The application includes features such as audio playback for pronunciation, various training modes, and progress tracking.

## Features

*   **Learn Thai Script:** Study and practice the Thai alphabet and script.
*   **Phonetic Transcription:** Learn the phonetic transcription of Thai words to aid in pronunciation.
*   **Vocabulary Builder:** Build your vocabulary with a collection of words and phrases organized by chapter.
*   **Audio Pronunciation:** Listen to the correct pronunciation of Thai words.
*   **Training Modes:** Test your knowledge with different training modes, including:
    *   Thai Script to English
    *   Phonetics to Thai Script
    *   English to Thai Script
*   **Progress Tracking:** Keep track of your learning progress with a points system and completed words list.
*   **Customizable Settings:** Adjust the application settings to your preferences, such as auto-play sounds, looping chapters, and randomizing words.
*   **Search Functionality:** Easily search for specific words within the vocabulary.
*   **Speech Recognition:** Practice your pronunciation with the speech recognition feature.

## Getting Started

### Prerequisites

*   [.NET Framework 4.6.1](https://www.microsoft.com/en-us/download/details.aspx?id=49982)
*   [Visual Studio](https://visualstudio.microsoft.com/vs/) (optional, for building from source)

### Installation

1.  Clone the repository:
    ```sh
    git clone https://github.com/your-username/LearnThaiApplication.git
    ```
2.  Navigate to the project directory:
    ```sh
    cd LearnThaiApplication
    ```
3.  Open the solution file `LearnThaiApplication.sln` in Visual Studio.
4.  Build the solution (Build > Build Solution).
5.  Run the application from Visual Studio or by executing the generated `.exe` file in the `bin/Debug` or `bin/Release` directory.

## Usage

1.  **Main Window:** The main window is divided into several tabs:
    *   **Learn:** This is the main tab for learning and practicing.
    *   **Content Management:** This tab allows you to view, edit, and delete words in the vocabulary.
    *   **Settings:** This tab allows you to configure the application settings.
2.  **Learning:**
    *   Select a chapter from the dropdown list to start learning.
    *   Use the "Next" and "Previous" buttons to navigate through the words in the chapter.
    *   Click the speaker icon to hear the pronunciation of the Thai word.
    *   Choose a training mode (e.g., "Train Script", "Train Fonet", "Train Words") to test your knowledge.
    *   Enter your answer in the textbox and click "Validate" to check your answer.
3.  **Settings:**
    *   **Auto Play Sounds:** Automatically play the sound for each word.
    *   **Loop Chapter:** Continuously loop through the words in the current chapter.
    *   **Randomize:** Display words in a random order.
    *   **Skip Completed Words:** Skip words that you have already learned.
    *   **Show Description:** Display a description of the current word.

## Data Files

The application uses XML files to store the vocabulary, chapters, and user data. These files are located in the `LearnThaiApplication/Files/Media/Language/` directory.

*   `Thai_Chapter.xml`: Contains the list of chapters.
*   `Thai_Word.xml`: Contains the list of words, including Thai script, phonetics, English translation, and other information.
*   `Users.xml`: Stores user profiles and their progress.

## Dependencies

This project relies on the following external libraries and packages:

*   [HtmlAgilityPack](https://html-agility-pack.net/): An agile HTML parser that builds a read/write DOM and supports plain XPATH or XSLT.
*   [NAudio](https://github.com/naudio/NAudio): An open source .NET audio and MIDI library.
*   [Newtonsoft.Json](https://www.newtonsoft.com/json): A popular high-performance JSON framework for .NET.
*   [Microsoft.WindowsAPICodePack-Core](https://www.nuget.org/packages/Microsoft.WindowsAPICodePack-Core/): Part of the Windows API Code Pack, providing access to Windows features.
*   [Microsoft.WindowsAPICodePack-Shell](https://www.nuget.org/packages/Microsoft.WindowsAPICodePack-Shell/): Part of the Windows API Code Pack, providing access to Windows Shell features.
