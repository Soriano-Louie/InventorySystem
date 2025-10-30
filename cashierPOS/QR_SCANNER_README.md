# QR Code Scanner Feature

## Overview
The QR Code Scanner feature allows cashiers to scan product QR codes using the laptop's built-in camera (not a barcode scanner device). When a QR code is scanned, the system validates it against the database and prompts for quantity before adding the product to the cart.

**Key Feature:** Manual resume control prevents accidental re-scanning of the same QR code, ensuring a smooth and controlled scanning workflow.

## Libraries Used
- **ZXing.Net** (v0.16.11) - QR code decoding library
- **ZXing.Net.Bindings.Windows.Compatibility** (v0.16.14) - Windows Forms compatibility bindings
- **AForge.Video** (v2.2.5) - Video capture framework
- **AForge.Video.DirectShow** (v2.2.5) - DirectShow video device access

## Features
1. **Live Camera Feed** - Displays real-time camera feed in the scanner window (760x480)
2. **Automatic QR Detection** - Continuously scans for QR codes in the camera view
3. **Database Validation** - Validates scanned QR codes against both `wholesaleProducts` and `retailProducts` tables
4. **Manual Resume Control** - Requires user to press SPACE or click Resume button after adding a product
5. **Processing Lock** - Prevents multiple simultaneous QR code processing
6. **Quantity Input** - Prompts user to enter quantity when a valid QR code is detected
   - Default value: 1
   - Range: 1-10,000
 - Text auto-selected for easy input
7. **Automatic Cart Addition** - Adds product to cart with proper pricing, VAT, and discount calculations
8. **Smart Scanning States** - Camera continues running but only processes frames when scanning is active
9. **Visual Feedback** - Green status text and visible Resume button after successful product addition

## How to Use
### Opening the QR Scanner
- **Button 2** in the POS Form
- **Keyboard Shortcut:** Press `F2` key

### Scanning Process
1. Open the QR Scanner window
2. Position the QR code in front of the camera
3. Wait for the system to detect and validate the QR code
4. **Camera feed continues** but stops processing new QR codes
5. Quantity dialog appears immediately (UI remains responsive)
6. Enter the desired quantity (text is auto-selected, just type the number)
7. Click OK or press Enter to add the product to the cart
8. Success message appears: "Added X x Product Name to cart!"
9. **Status turns GREEN** with message: "Product added! Remove QR code and press SPACE or click Resume to continue..."
10. **Green "Resume Scanning (SPACE)" button appears**
11. **Remove the QR code from camera view**
12. **Press SPACE key** or click "Resume Scanning" button to continue
13. Ready to scan the next item

### Quick Resume
- **SPACE key** - Fastest way to resume scanning
- **Click button** - Alternative method to resume

### Closing the Scanner
- Click the "Close (ESC)" button
- Press the `ESC` key
- **Maximum 2-second timeout** ensures form closes even if camera is busy

## Database Requirements
The QR Code Scanner validates against the `QRCodeImage` column (varbinary(max)) in both:
- `wholesaleProducts` table
- `retailProducts` table

The QR code images are stored as binary data in the database. The scanner:
1. Retrieves all products with non-null `QRCodeImage` values
2. Converts each binary image back to a bitmap
3. Decodes the QR code from each image
4. Compares the decoded text with the scanned QR code
5. Returns the matching product information

## Product Information Retrieved
When a valid QR code is found, the following information is retrieved:
- Product ID
- Product Name
- Retail Price (Unit Price)
- Category ID
- VAT Applicability

## Color Scheme
The QR Scanner form follows the application's color scheme:
- **Background:** RGB(224, 166, 109) - Light brown
- **Buttons:** RGB(230, 216, 177) - Cream
- **Resume Button:** RGB(144, 238, 144) - Light green
- **Text:** RGB(79, 51, 40) - Dark brown
- **Status Label (Normal):** Bold, centered, dark brown text
- **Status Label (Success):** Bold, centered, green text

## Error Handling
- **No Camera Detected:** Shows error message and closes the scanner
- **Invalid QR Code:** Shows warning and automatically resumes scanning
- **User Cancels Quantity:** Automatically resumes scanning without adding product
- **Camera Initialization Error:** Shows error message with details
- **Database Error:** Shows error message with details and resumes scanning
- **Form Closing Timeout:** Continues close operation after 2 seconds if camera doesn't stop

## Technical Implementation

### Thread Safety & Performance
- **Continuous camera feed** - Camera runs continuously without stopping/restarting
- **State-based processing** - Uses `isScanning` and `isProcessing` flags to control QR detection
- **Processing flag** - `isProcessing` boolean prevents multiple simultaneous QR processing
- **Dual state management** - `isScanning` controls detection, `isProcessing` prevents overlaps
- **Asynchronous frame updates** - Camera feed updates don't block UI
- **Form closing timeout** - Uses `Task.Run()` with 2-second timeout for graceful shutdown

### State Management
```vb
isScanning = True/False      ' Controls whether to detect QR codes in frames
isProcessing = True/False    ' Prevents multiple simultaneous processing
lastScannedCode = String     ' Cleared on manual resume to allow re-scanning
lastScanTime = DateTime      ' Timestamp of last scan for cooldown period
scanCooldownSeconds = 3      ' Cooldown period (3 seconds) between duplicate scans
btnResumeScan.Visible ' Shows/hides the resume button based on state
```

### Camera Lifecycle
1. **Initialization:** Camera starts when form loads and runs continuously
2. **Detection:** Processes frames when `isScanning=True` and `isProcessing=False`
3. **Product Found:** Sets `isScanning=False` to pause detection (camera keeps running)
4. **Dialog:** Shows quantity input while camera feed continues
5. **Success:** Shows green status and Resume button, keeps `isScanning=False`
6. **Manual Resume:** User presses SPACE or clicks button to set `isScanning=True`
7. **Cancel/Invalid:** Automatically sets `isScanning=True` to resume
8. **Close:** `SignalToStop()` + timeout-based `WaitForStop()` in background thread

### Scanning Flow States

#### After Successful Product Addition
- `isScanning` = `False` (detection paused)
- `isProcessing` = `False` (ready for user action)
- Status text = GREEN "Product added! Remove QR code and press SPACE..."
- Resume button = VISIBLE
- Camera feed = CONTINUES (shows live view)
- **Waits for user to press SPACE or click Resume button**

#### After Cancel or Invalid Code
- `isScanning` = `True` (automatically resumes)
- `isProcessing` = `False`
- Status text = NORMAL color
- Resume button = HIDDEN
- Camera feed = CONTINUES

### Key Improvements (v3.0)
- ? **Manual resume control** - Prevents accidental re-scanning of same QR code
- ? **Visual feedback** - Green status text and visible Resume button
- ? **SPACE key support** - Quick and easy resume with keyboard shortcut
- ? **Continuous camera feed** - No stop/restart, smoother experience
- ? **Smart auto-resume** - Automatically resumes only for cancel/invalid cases
- ? **Clear user guidance** - Status text tells user exactly what to do next
- ? **Cooldown period** - 3-second duplicate prevention as backup mechanism

### Key Improvements (v2.0)
- ? **Fixed UI freeze** - Removed blocking `WaitForStop()` from UI thread
- ? **Added processing lock** - Prevents overlapping QR code processing
- ? **Improved responsiveness** - Camera continues running, UI never freezes
- ? **Better error handling** - Proper exception catching for all operations
- ? **Graceful shutdown** - 2-second timeout prevents hanging on close
- ? **Auto-selected text** - Quantity input is immediately ready for typing

## Performance Characteristics
- **Camera FPS:** Typically 30 frames per second
- **QR decode:** On each frame when scanning active
- **Duplicate prevention:** 3-second cooldown per unique code (backup mechanism)
- **Manual resume:** Instant response to SPACE key or button click
- **Form close timeout:** 2000ms (2 seconds)
- **Auto-rotate enabled:** Better detection at various angles
- **TryHarder enabled:** More thorough QR code detection

## Technical Notes
- The scanner uses the first available camera device
- Frame processing errors are silently ignored to prevent crashes
- The video source is properly cleaned up when the form closes
- The scanner window is modal (blocks parent form interaction)
- Camera resources are automatically released on form close
- **UI thread safety:** All camera operations handle `ObjectDisposedException` and `InvalidOperationException`
- **Memory management:** Frames are properly disposed after processing
- **Continuous operation:** Camera never stops between scans, only detection pauses

## Troubleshooting

### Issue: Same QR code scans repeatedly
**Status:** FIXED in v3.0
**Solution:** Manual resume control requires user to press SPACE or click Resume after each scan

### Issue: Form freezes after scanning
**Status:** FIXED in v2.0
**Solution:** Removed blocking `WaitForStop()` call from UI thread

### Issue: Multiple dialogs appear
**Status:** FIXED in v2.0
**Solution:** Added `isProcessing` flag to prevent simultaneous processing

### Issue: Form won't close
**Status:** FIXED in v2.0
**Solution:** Added 2-second timeout mechanism for camera stop operation

### Issue: Resume button doesn't work
**Check:** Make sure you're clicking the green "Resume Scanning (SPACE)" button
**Alternative:** Press the SPACE key instead

## Future Enhancements
- Support for multiple camera selection
- Adjustable scan cooldown period (currently 3 seconds)
- Scan history log with timestamps
- QR code generation for products without codes
- Manual camera focus adjustment
- Camera resolution settings
- Barcode support (in addition to QR codes)
- Sound/visual feedback on successful scan
- Auto-resume after configurable delay

## Version History

### v3.0 (January 2025)
- **Added manual resume control** - Prevents accidental re-scanning
- Added green "Resume Scanning (SPACE)" button
- Added SPACE key shortcut for quick resume
- Green status text feedback after successful addition
- Camera now runs continuously without stop/restart
- Clear last scanned code on resume to allow re-scanning same product
- Auto-resume only for cancel/invalid cases
- Improved user guidance with descriptive status messages

### v2.0 (January 2025)
- Fixed UI freeze issue during QR code processing
- Added processing lock to prevent overlapping operations
- Improved camera pause/resume mechanism
- Enhanced error handling and recovery
- Added timeout-based form closing
- Improved quantity input UX (auto-select text)

### v1.0 (Initial Release)
- Basic QR code scanning functionality
- Database validation
- Quantity input dialog
- Cart integration

## User Tips
?? **Pro Tip:** Use the SPACE key for fastest scanning workflow - scan, enter quantity, press ENTER, remove QR code, press SPACE, repeat!

?? **Workflow:** The green Resume button is your signal to remove the QR code before continuing

?? **Speed:** Keep QR codes ready and use keyboard shortcuts (ENTER for OK, SPACE for Resume) for fastest checkout
