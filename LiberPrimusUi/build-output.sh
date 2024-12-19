#!/bin/bash

# Function to perform the build
perform_build() {
    # Prompt for the release number
    read -p "Enter the release number: " RELEASE_NUMBER

    # Get the current directory
    CURRENT_DIR=$(pwd)

    # Define the project path
    PROJECT_PATH="$CURRENT_DIR/LiberPrimusUi.csproj"

    # Define the output directory
    OUTPUT_DIR="$CURRENT_DIR/output"

    # Remove the output directory if it does exist
    rm -rvf "$OUTPUT_DIR"

    # Create the output directory if it does not exist
    mkdir -p "$OUTPUT_DIR"

    # Define the runtime identifiers
    RIDS=("osx-x64" "win-x64" "linux-x64")

    # Loop through each runtime identifier and publish the project
    for RID in "${RIDS[@]}"; do
        echo "Publishing for $RID..."
        dotnet publish "$PROJECT_PATH" -c Release -r "$RID" --self-contained -o "$OUTPUT_DIR/$RID"
        if [ $? -ne 0 ]; then
            echo "Failed to publish for $RID"
            exit 1
        fi
        cd $OUTPUT_DIR && zip -r "${RID}_release_${RELEASE_NUMBER}.zip" "$RID"
        if [ $? -ne 0 ]; then
            echo "Failed to zip $PUBLISH_DIR"
            exit 1
        fi
        cd $CURRENT_DIR
        ##rm -rvf "$OUTPUT_DIR/$RID"
    done

    dotnet clean --verbosity diag

    echo "Publishing completed successfully."
}

# Function to perform the clean
perform_clean() {
    # Get the current directory
    CURRENT_DIR=$(pwd)

    # Define the output directory
    OUTPUT_DIR="$CURRENT_DIR/output"

    # Remove the output directory if it does exist
    rm -rvf "$OUTPUT_DIR"

    dotnet clean --verbosity diag

    echo "Clean completed successfully."
}

# Prompt for the option
echo "Select an option:"
echo "1. Build and Clean"
echo "2. Clean Only"
read -p "Enter your choice (1 or 2): " OPTION

case $OPTION in
    1)
        perform_build
        ;;
    2)
        perform_clean
        ;;
    *)
        echo "Invalid option"
        exit 1
        ;;
esac