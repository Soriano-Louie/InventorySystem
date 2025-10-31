# QR Code Scanner Feature

## Overview
The QR Code Scanner feature allows cashiers to scan product QR codes using the laptop's built-in camera (not a barcode scanner device). When a QR code is scanned, the system validates it against the database and **automatically adds 1 unit** to the cart - enabling fast, continuous scanning just like traditional barcode scanners at retail checkouts.

**Key Feature:** Continuous automatic scanning - scan multiple products in rapid succession without interruption. Scanning the same QR code multiple times increases the quantity automatically.

## Libraries Used
- **ZXing.Net** (v0.16.11) - QR code decoding library
- **ZXing.Net.Bindings.Windows.Compatibility** (v0.16.14) - Windows Forms compatibility bindings
- **AForge.Video** (v2.2.5) - Video capture framework
- **AForge.Video.DirectShow** (v2.2.5) - DirectShow video device access

## Features
1. **Live Camera Feed** - Displays real-time camera feed in the scanner window (760x480)
2. **Automatic QR Detection** - Continuously scans for QR codes in the camera view
3. **Instant Cart Addition** - Each scan automatically adds 1 unit to cart (no prompts)
4. **Database Validation** - Validates scanned QR codes against both `wholesaleProducts` and `retailProducts` tables
5. **Duplicate Detection** - 1-second cooldown prevents accidental double-scans of the same QR code
6. **Automatic Quantity Increase** - Scanning the same product multiple times increases quantity in cart
7. **Visual Feedback** - Color-coded status messages (Blue=Detecting, Green=Added, Red=Invalid)
8. **Processing Lock** - Prevents multiple simultaneous QR code processing
9. **Continuous Operation** - Camera runs continuously, no manual resume needed
10. **Auto-Resume** - Automatically ready for next scan after each product

## How to Use
### Opening the QR Scanner
- **Button 2** in the POS Form
- **Keyboard Shortcut:** Press `F2` key

### Scanning Process (Continuous Mode)
1. Open the QR Scanner window
2. **Ready to scan** - Status shows "Ready to scan QR code..."
3. Hold QR code in front of camera
4. **Detecting** - Status turns BLUE "QR Code detected! Validating..."
5. **Success** - Status turns GREEN "Added: [Product Name]" (300ms pause)
6. **Auto-ready** - Status returns to normal "Ready to scan next QR code..."
7. **Repeat** - Immediately scan next product (no button press needed)

**Note:** During the 300ms success feedback and 500ms error feedback periods, the UI will pause briefly to show the status message.

### Quantity Management
- **First scan** - Adds product to cart with quantity = 1
- **Second scan (same product)** - Increases quantity to 2
- **Third scan (same product)** - Increases quantity to 3
- **And so on...** Each additional scan increases quantity by 1
- **Cooldown:** 1-second between scans of the same QR code to prevent accidental duplicates

### Visual Feedback
- ?? **Normal** (Dark Brown): Ready to scan
- ?? **Blue**: Detecting/Validating QR code
- ?? **Green**: Product added successfully (shows for 300ms)
- ?? **Red**: Invalid QR code (shows for 500ms)

### Invalid QR Code Handling
- Shows "Invalid QR Code! Scan again..." in RED
- Brief 500ms pause to display error message
- Automatically resumes scanning
- No user action required

### Closing the Scanner
- Click the "Close (ESC)" button (centered at bottom)
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
- **Text:** RGB(79, 51, 40) - Dark brown
- **Status Label Colors:**
  - Normal: Dark brown RGB(79, 51, 40)
  - Detecting: Blue (Color.Blue)
  - Success: Green (Color.Green)
  - Error: Red (Color.Red)

## Error Handling
- **No Camera Detected:** Shows error message and closes the scanner
- **Invalid QR Code:** Shows red status message with 500ms pause, auto-resumes
- **Camera Initialization Error:** Shows error message with details and closes scanner
- **Database Error:** Shows error message dialog with details and resumes scanning
- **Processing Error:** Shows error message dialog, resumes scanning
- **Form Closing Timeout:** Continues close operation after 2 seconds if camera doesn't stop

## Technical Implementation

### Thread Safety & Performance
- **Continuous camera feed** - Camera runs continuously without stopping
- **State-based processing** - Uses `isScanning` and `isProcessing` flags to control QR detection
- **Processing lock** - `isProcessing` boolean prevents multiple simultaneous QR processing
- **Asynchronous frame updates** - Camera feed updates on UI thread via `Invoke()`
- **Brief UI pause** - 300ms for success, 500ms for errors using `Thread.Sleep()` on UI thread
- **Form closing timeout** - Uses `Task.Run()` with 2-second timeout for graceful shutdown

### State Management
```vb
isScanning = True/False      ' Controls whether to detect QR codes in frames
isProcessing = True/False    ' Prevents multiple simultaneous processing
lastScannedCode = String     ' Last scanned code for duplicate prevention
lastScanTime = DateTime      ' Timestamp of last scan for cooldown period
scanCooldownSeconds = 1      ' Fast 1-second cooldown for continuous scanning
```

### Scanning Flow
1. **Ready State:** `isScanning=True`, `isProcessing=False`, waiting for QR code
2. **Detection:** QR code found ? `isScanning=False`, `isProcessing=True`
3. **Validation:** Check against database (synchronous on UI thread)
4. **Add to Cart:** Call `parentForm.AddProductToCart()` with quantity=1
5. **Visual Feedback:** Status shows GREEN with product name (300ms pause via Thread.Sleep)
6. **Auto-Resume:** `isScanning=True`, `isProcessing=False` ? ready for next scan

### Processing Details
```vb
' When QR code detected:
isScanning = False   ' Stop processing new frames
labelStatus.ForeColor = Blue    ' Show detecting status

' If valid product:
parentForm.AddProductToCart(...)    ' Add with quantity=1
labelStatus.ForeColor = Green       ' Show success
Thread.Sleep(300)         ' Brief pause (UI freezes)
labelStatus.ForeColor = DarkBrown   ' Reset to normal
isScanning = True  ' Resume scanning

' If invalid code:
labelStatus.ForeColor = Red         ' Show error
Thread.Sleep(500)  ' Brief pause (UI freezes)
labelStatus.ForeColor = DarkBrown   ' Reset to normal
isScanning = True            ' Resume scanning
```

### Key Improvements (v4.0 - Continuous Scanning Mode)
- ? **Removed quantity dialog** - No interruptions, fully automatic
- ? **Removed manual resume button** - Automatically ready after each scan
- ? **Removed success MessageBox** - Silent operation with status label feedback only
- ? **Fixed quantity = 1 per scan** - Like traditional barcode scanners
- ? **Reduced cooldown to 1 second** - Faster continuous scanning
- ? **Color-coded feedback** - Blue/Green/Red status for instant visual confirmation
- ? **Auto-resume on error** - Invalid codes don't stop workflow
- ? **Simplified UI** - Only Close button needed (centered)
- ? **Retail-style workflow** - Scan-scan-scan without pauses or dialogs
- ? **Brief status pauses** - 300ms success, 500ms error for user feedback

### Key Improvements (v3.0)
- ? **Manual resume control** - Prevents accidental re-scanning
- ? **Visual feedback** - Green status text and visible Resume button
- ? **SPACE key support** - Quick and easy resume with keyboard shortcut
- ? **Continuous camera feed** - No stop/restart, smoother experience

### Key Improvements (v2.0)
- ? **Fixed UI freeze** - Removed blocking `WaitForStop()` from UI thread
- ? **Added processing lock** - Prevents overlapping QR code processing
- ? **Improved responsiveness** - Camera continues running, UI never freezes

## Performance Characteristics
- **Camera FPS:** Typically 30 frames per second
- **QR decode:** On each frame when `isScanning=True` and `isProcessing=False`
- **Duplicate prevention:** 1-second cooldown (fast for continuous scanning)
- **Success feedback:** 300ms UI pause with green status
- **Error feedback:** 500ms UI pause with red status
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
- **Continuous operation:** Camera never stops, only detection pauses briefly during processing
- **Thread.Sleep on UI thread:** Brief pauses (300ms/500ms) for visual feedback - UI is unresponsive during these periods
- **No popup dialogs:** Success feedback shown only via status label, no MessageBox interruptions

## Troubleshooting

### Issue: UI freezes briefly after each scan
**Status:** Expected behavior
**Explanation:** 300ms pause after success and 500ms after error are intentional for visual feedback
**Impact:** Very brief, allows user to see status color change

### Issue: Same product scans too quickly
**Solution:** 1-second cooldown prevents duplicate scans. Wait 1 second to scan same QR code again
**Why:** Prevents accidental double-scans when holding QR code steady

### Issue: Quantity not updating correctly
**Check:** The cart's `AddProductToCart` method in `posForm.vb` should handle updating quantity for existing products
**Expected Behavior:** 
  - First scan of Product A ? Quantity: 1
  - Second scan of Product A ? Quantity: 2 (updates existing row)
  - Scan of Product B ? Quantity: 1 (new row)

### Issue: Scanner too slow
**Causes:** 
- Poor lighting conditions
- QR code too small or too far from camera
- QR code damaged, faded, or blurry
- Camera quality or focus issues
**Solutions:**
- Improve lighting (overhead lights, desk lamp)
- Hold QR code closer (optimal: 15-30cm from camera)
- Ensure QR code is clear, high contrast, and undamaged
- Clean camera lens if dirty

### Issue: Camera not starting
**Check:** 
- Ensure camera is not in use by another application
- Check camera permissions in Windows settings
- Verify camera drivers are installed
- Try restarting the application

### Issue: Form won't close
**Status:** FIXED in v2.0
**Solution:** Added 2-second timeout mechanism for camera stop operation
**Fallback:** Force close after 2 seconds if camera doesn't respond

## Retail Checkout Workflow

### Fast Scanning Tips
1. **Keep QR codes ready** - Have products lined up with QR codes visible
2. **Optimal distance** - 15-30cm from camera
3. **Good lighting** - Ensure adequate light on QR codes (natural or artificial)
4. **Steady hold** - Hold QR code steady for 0.5-1 second until you see GREEN
5. **Watch status color** - Wait for GREEN feedback before moving to next product
6. **Rapid succession** - Can scan different products immediately after GREEN status
7. **Same product** - Wait 1 second between scans of the same QR code
8. **Positioning** - Center QR code in camera view for best detection

### Example Checkout Session
```
Customer purchases: 2x Coffee, 1x Bread, 2x Milk

Workflow:
1. Scan Coffee QR ? BLUE ? GREEN "Added: Coffee" ? Qty: 1
2. Wait 1 sec ? Scan Coffee QR again ? BLUE ? GREEN "Added: Coffee" ? Qty: 2
3. Scan Bread QR ? BLUE ? GREEN "Added: Bread" ? Qty: 1
4. Scan Milk QR ? BLUE ? GREEN "Added: Milk" ? Qty: 1
5. Wait 1 sec ? Scan Milk QR again ? BLUE ? GREEN "Added: Milk" ? Qty: 2
6. Close scanner ? Process payment

Total Time: ~10-15 seconds for 5 scans
```

### Optimal Scanning Speed
- **Different products:** Scan immediately after GREEN (no cooldown)
- **Same product:** Wait 1 second after GREEN before next scan
- **Average speed:** 2-3 scans per minute (with proper technique)
- **Maximum speed:** Up to 30-40 scans per minute for different products

## Common User Errors

### Scanning too fast
**Issue:** Same QR code scanned twice within 1 second
**Result:** Second scan ignored (cooldown active)
**Solution:** Wait for GREEN status, pause 1 second, then scan again

### Moving QR code while scanning
**Issue:** QR code moved away before detection completes
**Result:** No detection or invalid read
**Solution:** Hold QR code steady until GREEN status appears

### Poor QR code positioning
**Issue:** QR code at edge of camera view or tilted
**Result:** Slow detection or no detection
**Solution:** Center QR code in camera view, hold perpendicular to camera

## Future Enhancements
- ?? Async/await pattern for non-blocking status feedback
- ?? Support for multiple camera selection
- ?? Adjustable scan cooldown period in settings
- ?? Sound feedback on successful scan (beep)
- ?? Scan counter display (items scanned this session)
- ?? Scan history log with timestamps
- ??? QR code generation for products without codes
- ?? Manual camera focus adjustment
- ?? Camera resolution settings
- ?? Barcode support (in addition to QR codes, e.g., EAN-13, UPC)
- ?? Haptic feedback support
- ?? Visual target overlay on camera feed
- ?? Scanning performance metrics

## Version History

### v4.0 (January 2025) - Continuous Scanning Mode
- **Removed quantity dialog** - Fully automatic scanning without prompts
- **Removed manual resume button** - Auto-resumes after each scan
- **Removed success MessageBox** - Silent operation with status label only
- **Fixed quantity at 1 per scan** - Traditional scanner behavior
- Reduced cooldown to 1 second for faster scanning
- Added color-coded status feedback (Blue/Green/Red)
- Simplified UI - Only Close button remains (centered)
- Added brief pauses for visual feedback (300ms success, 500ms error)
- Retail-style continuous scanning workflow
- Updated README with actual implementation details

### v3.0 (January 2025)
- **Added manual resume control** - Prevents accidental re-scanning
- Added green "Resume Scanning (SPACE)" button
- Added SPACE key shortcut for quick resume
- Green status text feedback after successful addition
- Camera now runs continuously without stop/restart
- Clear last scanned code on resume to allow re-scanning

### v2.0 (January 2025)
- Fixed UI freeze issue during QR code processing
- Added processing lock to prevent overlapping operations
- Enhanced error handling and recovery
- Added timeout-based form closing
- Improved quantity input UX (auto-select text)

### v1.0 (Initial Release)
- Basic QR code scanning functionality
- Database validation
- Quantity input dialog
- Cart integration

## User Tips
?? **Pro Tip:** Scan rapidly like a retail checkout - the system handles everything automatically!

?? **Speed Workflow:** Position products ready ? Scan ? Watch for GREEN ? Next product

?? **Multiple Units:** For bulk items, scan the same QR code multiple times (1 second apart)

?? **Error Recovery:** Red status? Just scan again - no action needed!

?? **Optimal Performance:** Good lighting + steady hands + 15-30cm distance = fastest scanning

?? **Visual Feedback:** Trust the colors - Blue=Working, Green=Success, Red=Error

?? **Cooldown Awareness:** Different products scan instantly; same product needs 1 second wait

?? **Camera Positioning:** Keep scanner window visible to monitor status changes
