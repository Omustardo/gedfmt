#!/bin/bash

# Script to build releases. Runs on the linux subsystem on Windows.
# It requires the visual basic compiler (vbc.exe) to be added to the windows Path environmental variable.

# Remove all existing releases and build artifacts.
clean_build () {
  rm -rf ./.releases

  go clean
}

get_windows_desktop () {
  echo "*************************************************"
  echo "*** Retrieving executable for Windows Desktop ***"
  mkdir -p ./.releases/windows_desktop/
  cp "./windows_desktop/WikiTree Bio/WikiTree Text Formatter/bin/x86/Release/WikiTree Text Formatter.exe" "./.releases/windows_desktop/WikiTree_Text_Formatter.exe"
  ls ./.releases/windows_desktop/
  echo ""
}

# Build the Windows gedfmt binary.
build_windows_gedfmt () {
  echo "**********************************************"
  echo "*** Building gedfmt executable for Windows ***"
  mkdir -p ./.releases/windows_gedfmt/
  # Build and move the windows formatter script.
  cd windows_gedfmt
  vbc.exe -optimize gedfmt.vbs *.vb
  cd ..
  mv ./windows_gedfmt/gedfmt.exe ./.releases/windows_gedfmt/gedfmt.exe
  ls ./.releases/windows_gedfmt/
  echo ""
}

# Build the binary and all supporting files needed to run a gedfmt server on Windows.
build_windows_server_variant () {
  local GOARCH=$1
  echo "**********************************************"
  echo "*** Building server for Windows Arch=$GOARCH ***"
  local SUFFIX=".exe"

  mkdir -p ./.releases/.tmp/windows_"$GOARCH"_server/

  # Build the server binary.
  go build -o ./.releases/.tmp/windows_"$GOARCH"_server/server"$SUFFIX" ./server/main.go

  # Build and move the windows formatter script.
  cd windows_gedfmt
  vbc.exe -optimize gedfmt.vbs *.vb
  cd ..
  mv ./windows_gedfmt/gedfmt.exe ./.releases/.tmp/windows_"$GOARCH"_server/gedfmt.exe

  # Copy UI
  cp ./server/gedfmt.html ./.releases/.tmp/windows_"$GOARCH"_server/gedfmt.html

  # TODO: Copy documentation for the server into the ouput directory.

  # Copy output to a tar.gz file.
  # -C allows changing the directory before adding to the archive, which avoids unnecessary nesting of directories within
  # the archive.
  tar -C ./.releases/.tmp/windows_"$GOARCH"_server -czvf ./.releases/windows_"$GOARCH"_server.tar.gz .

  echo ""
}

clean_build

get_windows_desktop
build_windows_gedfmt
build_windows_server_variant amd64

rm -rf ./.releases/.tmp
