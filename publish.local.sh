#!/bin/bash

# Define variables
PROJECT_NAME="Instagram"
OUTPUT_DIR="bin/Release"
PACKAGE_DIR="Packages"

# Create package directory if it doesn't exist
mkdir -p $PACKAGE_DIR

# Build the project in Release mode
dotnet build -c Release

# Check if the build was successful
if [ $? -ne 0 ]; then
    echo "Build failed. Exiting."
    exit 1
fi

# Pack the project
dotnet pack -c Release

# Check if the pack was successful
if [ $? -ne 0 ]; then
    echo "Packing failed. Exiting."
    exit 1
fi

# Copy the .nupkg file to the Packages directory
cp $OUTPUT_DIR/$PROJECT_NAME.*.nupkg $PACKAGE_DIR/

# Check if the copy was successful
if [ $? -ne 0 ]; then
    echo "Copying the package failed. Exiting."
    exit 1
fi

echo "Package published locally to $PACKAGE_DIR"