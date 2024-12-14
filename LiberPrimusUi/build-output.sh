#!/bin/bash

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
    cd $OUTPUT_DIR && zip -r "$RID.zip" "$RID"
    if [ $? -ne 0 ]; then
        echo "Failed to zip $PUBLISH_DIR"
        exit 1
    fi
    cd $CURRENT_DIR
    rm -rvf "$OUTPUT_DIR/$RID"
done

echo "Publishing for linux-x64..."
dotnet publish "$PROJECT_PATH" -c Release -r "linux-x64" --self-contained -o "$OUTPUT_DIR/linux-x64"
if [ $? -ne 0 ]; then
    echo "Failed to publish for linux-x64"
    exit 1
fi

dotnet clean --verbosity diag

echo "Publishing completed successfully."
