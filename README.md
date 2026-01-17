# Voice Recording & Transcription Tool

A simple C# console application that records audio and transcribes it using the LemonFox AI API.

## Features

- Record audio from your microphone using `arecord`
- Stop recording and automatically transcribe the audio
- Uses LemonFox AI's Whisper API for speech-to-text conversion

## Prerequisites

- .NET 6.0 or higher
- Linux operating system (uses `arecord` command)
- ALSA audio utilities installed
- LemonFox AI API key

## Installation

1. Clone or download this project

2. Install ALSA utilities if not already installed:
```bash
sudo apt-get install alsa-utils
```

3. Create an `appsetting.json` file in the project directory:
```json
{
  "apiKey": {
    "lemonfox": "your-api-key-here"
  }
}
```

4. Update the `SetBasePath` in `Program.cs` to match your project directory

## Usage

### Start Recording
```bash
dotnet run record
```

This will start recording audio from your default microphone and save it to `/home/tojan/record.wav`.

### Stop Recording & Transcribe
```bash
dotnet run stop
```

This will:
1. Stop the recording process
2. Send the audio file to LemonFox AI for transcription
3. Display the transcribed text in the console
4. Clean up temporary files

## Configuration

### File Paths

You may need to update these paths in `Program.cs`:

- **Config path**: Line 13 - Update to your project directory
- **Recording file**: Line 15 - Change output location if desired
- **PID file**: Line 16 - Temporary file to track recording process

### Audio Format

The default recording format is CD quality (44.1kHz, 16-bit, stereo). To change this, modify the `Arguments` on line 19.

## How It Works

1. **Recording**: Uses `arecord` to capture audio and saves the process ID to a temporary file
2. **Stopping**: Reads the process ID, kills the recording process, then sends the audio file to the API
3. **Transcription**: The LemonFox AI Whisper API processes the audio and returns the transcribed text

## Error Handling

- If no recording is running when you try to stop, you'll see: "No PID file found"
- If the process ID is invalid: "No pid found"

## Security Note

⚠️ **Important**: Never commit your `appsetting.json` file with your actual API key to version control. Add it to `.gitignore`:
```
appsetting.json
```

## API Reference

This project uses the LemonFox AI Audio Transcription API:
- **Endpoint**: `https://api.lemonfox.ai/v1/audio/transcriptions`
- **Model**: whisper-1
- **Format**: multipart/form-data

## License

All rights reserved. This project is for personal use only.

## Troubleshooting

**Recording doesn't start**
- Check that `arecord` is installed: `which arecord`
- Test your microphone: `arecord -d 5 test.wav`

**Transcription fails**
- Verify your API key is correct
- Check your internet connection
- Ensure the audio file was created successfully

**Permission errors**
- Make sure you have write permissions to `/home/tojan/` and `/tmp/`
