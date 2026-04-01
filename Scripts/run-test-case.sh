#!/bin/bash

#  run-test-case.sh
#  Contentstack
#
#  Created by Uttam Ukkoji on 12/04/21.
#  Copyright © 2026 Contentstack. All rights reserved.

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
cd "$REPO_ROOT" || exit 1

TEST_PROJECT="Contentstack.Utils.Tests"
TEST_RESULTS_DIR="$REPO_ROOT/$TEST_PROJECT/TestResults"

echo "Removing files"
rm -rf "$TEST_RESULTS_DIR"

DATE=$(date +'%d-%b-%Y')
FILE_NAME="Contentstack-DotNet-Test-Case-$DATE"

echo "Running test case..."
dotnet test "$REPO_ROOT/Contentstack.Utils.sln" \
  --logger "trx;LogFileName=Report-$FILE_NAME.trx" \
  --collect:"XPlat code coverage"

echo "Test case Completed..."

echo "Generating code coverage report..."

reports=$(find "$TEST_RESULTS_DIR" -name "coverage.cobertura.xml" 2>/dev/null | paste -sd ';' -)
if [ -z "$reports" ]; then
  reports=$(find "$REPO_ROOT/TestResults" -name "coverage.cobertura.xml" 2>/dev/null | paste -sd ';' -)
fi

if [ -z "$reports" ]; then
  echo "No coverage.cobertura.xml found under $TEST_RESULTS_DIR or $REPO_ROOT/TestResults."
  exit 1
fi

COVERAGE_OUT="$TEST_RESULTS_DIR/Coverage-$FILE_NAME"
mkdir -p "$COVERAGE_OUT"

TRX_FILE="$TEST_RESULTS_DIR/Report-$FILE_NAME.trx"
COVERAGE_FILE="${reports%%;*}"

python3 "$REPO_ROOT/Scripts/generate_test_report.py" \
  "$TRX_FILE" \
  --coverage "$COVERAGE_FILE" \
  --output "$COVERAGE_OUT/index.html"

echo ""
echo "Code coverage report generated."
echo "Open: $COVERAGE_OUT/index.html"
