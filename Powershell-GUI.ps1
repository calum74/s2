# Load required .NET assemblies for Windows Forms and Drawing.
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

# --------------------------------------------------
# Create Main Form
# --------------------------------------------------
$form = New-Object System.Windows.Forms.Form
$form.Text = "S2 Control GUI"
# Adjusted size to fit all sections.
$form.Size = New-Object System.Drawing.Size(820, 840)
$form.StartPosition = "CenterScreen"

# --------------------------------------------------
# S2 Executable Section
# --------------------------------------------------
$s2Group = New-Object System.Windows.Forms.GroupBox
$s2Group.Text = "s2 Executable"
$s2Group.Location = New-Object System.Drawing.Point(10,10)
$s2Group.Size = New-Object System.Drawing.Size(780,60)
$form.Controls.Add($s2Group)

$labelS2 = New-Object System.Windows.Forms.Label
$labelS2.Location = New-Object System.Drawing.Point(10,25)
$labelS2.Size = New-Object System.Drawing.Size(70,20)
$labelS2.Text = "Exe Path:"
$s2Group.Controls.Add($labelS2)

$textS2 = New-Object System.Windows.Forms.TextBox
$textS2.Location = New-Object System.Drawing.Point(90,23)
$textS2.Size = New-Object System.Drawing.Size(550,20)
# Update this default path as needed.
$textS2.Text = "C:\Users\owner\Downloads\s2\s2\build\Release\s2.exe"
$s2Group.Controls.Add($textS2)

$buttonS2Browse = New-Object System.Windows.Forms.Button
$buttonS2Browse.Location = New-Object System.Drawing.Point(650,20)
$buttonS2Browse.Size = New-Object System.Drawing.Size(100,24)
$buttonS2Browse.Text = "Browse..."
$s2Group.Controls.Add($buttonS2Browse)

$openFileDialogExe = New-Object System.Windows.Forms.OpenFileDialog
$openFileDialogExe.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*"
$buttonS2Browse.Add_Click({
    if ($openFileDialogExe.ShowDialog() -eq [System.Windows.Forms.DialogResult]::OK) {
        $textS2.Text = $openFileDialogExe.FileName
    }
})

# --------------------------------------------------
# Spooky2 Installation Section
# --------------------------------------------------
$spookyGroup = New-Object System.Windows.Forms.GroupBox
$spookyGroup.Text = "Spooky2 Installation"
$spookyGroup.Location = New-Object System.Drawing.Point(10,80)
$spookyGroup.Size = New-Object System.Drawing.Size(780,60)
$form.Controls.Add($spookyGroup)

$labelSpooky = New-Object System.Windows.Forms.Label
$labelSpooky.Location = New-Object System.Drawing.Point(10,25)
$labelSpooky.Size = New-Object System.Drawing.Size(90,20)
$labelSpooky.Text = "Spooky2 Path:"
$spookyGroup.Controls.Add($labelSpooky)

$textSpooky = New-Object System.Windows.Forms.TextBox
$textSpooky.Location = New-Object System.Drawing.Point(110,23)
$textSpooky.Size = New-Object System.Drawing.Size(450,20)
# Set default path as needed. (If not needed by s2.exe, you can leave it blank.)
$textSpooky.Text = "C:\Spooky2"
$spookyGroup.Controls.Add($textSpooky)

$buttonSpookyBrowse = New-Object System.Windows.Forms.Button
$buttonSpookyBrowse.Location = New-Object System.Drawing.Point(570,20)
$buttonSpookyBrowse.Size = New-Object System.Drawing.Size(100,24)
$buttonSpookyBrowse.Text = "Browse..."
$spookyGroup.Controls.Add($buttonSpookyBrowse)

$buttonSpookyOpen = New-Object System.Windows.Forms.Button
$buttonSpookyOpen.Location = New-Object System.Drawing.Point(680,20)
$buttonSpookyOpen.Size = New-Object System.Drawing.Size(80,24)
$buttonSpookyOpen.Text = "Open Folder"
$spookyGroup.Controls.Add($buttonSpookyOpen)

$folderBrowser = New-Object System.Windows.Forms.FolderBrowserDialog
$buttonSpookyBrowse.Add_Click({
    if ($folderBrowser.ShowDialog() -eq [System.Windows.Forms.DialogResult]::OK) {
        $textSpooky.Text = $folderBrowser.SelectedPath
    }
})
$buttonSpookyOpen.Add_Click({
    if (Test-Path $textSpooky.Text) {
        Invoke-Item $textSpooky.Text
    } else {
        [System.Windows.Forms.MessageBox]::Show("Folder not found: $($textSpooky.Text)", "Error", [System.Windows.Forms.MessageBoxButtons]::OK, [System.Windows.Forms.MessageBoxIcon]::Error)
    }
})

# --------------------------------------------------
# Variables Section (Command Parameters)
# --------------------------------------------------
$varGroup = New-Object System.Windows.Forms.GroupBox
$varGroup.Text = "Variables (Command Parameters)"
# Placed below the Spooky2 section.
$varGroup.Location = New-Object System.Drawing.Point(10,150)
$varGroup.Size = New-Object System.Drawing.Size(780,280)
$form.Controls.Add($varGroup)

# Helper function: adds a label and control pair to $varGroup.
function Add-LabeledControl {
    param (
        [string]$labelText,
        [System.Windows.Forms.Control]$control,
        [int]$x,
        [int]$y,
        [int]$labelWidth = 120,
        [int]$controlWidth = 150
    )
    $lbl = New-Object System.Windows.Forms.Label
    $lbl.Location = New-Object System.Drawing.Point($x, $y)
    $lbl.Size = New-Object System.Drawing.Size($labelWidth, 20)
    $lbl.Text = $labelText
    $varGroup.Controls.Add($lbl)
    $control.Location = New-Object System.Drawing.Point(($x + $labelWidth), $y)
    $control.Size = New-Object System.Drawing.Size($controlWidth, 20)
    $varGroup.Controls.Add($control)
}

# Row 1: Amplitude and Channel
$txtAmplitude = New-Object System.Windows.Forms.TextBox
Add-LabeledControl -labelText "Amplitude (e.g., 5V)" -control $txtAmplitude -x 10 -y 30

$txtChannel = New-Object System.Windows.Forms.TextBox
Add-LabeledControl -labelText "Channel (0,1,2)" -control $txtChannel -x 300 -y 30

# Row 2: Duration and Frequency
$txtDuration = New-Object System.Windows.Forms.TextBox
Add-LabeledControl -labelText "Duration (s,m,h)" -control $txtDuration -x 10 -y 70

$txtFrequency = New-Object System.Windows.Forms.TextBox
Add-LabeledControl -labelText "Frequency (e.g., 500kHz)" -control $txtFrequency -x 300 -y 70

# Row 3: Generator (ComboBox) and Pulse
$cboGenerator = New-Object System.Windows.Forms.ComboBox
$cboGenerator.DropDownStyle = "DropDownList"
# Add possible generator IDs 0 through 9.
0..9 | ForEach-Object { $cboGenerator.Items.Add($_.ToString()) }
# Set default generator (for example, "3")
$cboGenerator.SelectedItem = "3"
Add-LabeledControl -labelText "Generator" -control $cboGenerator -x 10 -y 110

$txtPulse = New-Object System.Windows.Forms.TextBox
Add-LabeledControl -labelText "Pulse" -control $txtPulse -x 300 -y 110

# Row 4: Simulation and Waveform
$cboSimulation = New-Object System.Windows.Forms.ComboBox
$cboSimulation.DropDownStyle = "DropDownList"
$cboSimulation.Items.Add("on")
$cboSimulation.Items.Add("off")
$cboSimulation.SelectedIndex = 1
Add-LabeledControl -labelText "Simulation" -control $cboSimulation -x 10 -y 150

$cboWaveform = New-Object System.Windows.Forms.ComboBox
$cboWaveform.DropDownStyle = "DropDownList"
$cboWaveform.Items.AddRange(@("sine","square","triangle","saw","rsaw","hbomb"))
$cboWaveform.SelectedIndex = 0
Add-LabeledControl -labelText "Waveform" -control $cboWaveform -x 300 -y 150

# Row 5: Loop and Iterations
$cboLoop = New-Object System.Windows.Forms.ComboBox
$cboLoop.DropDownStyle = "DropDownList"
$cboLoop.Items.Add("on")
$cboLoop.Items.Add("off")
$cboLoop.SelectedIndex = 1
Add-LabeledControl -labelText "Loop" -control $cboLoop -x 10 -y 190

$txtIterations = New-Object System.Windows.Forms.TextBox
Add-LabeledControl -labelText "Iterations" -control $txtIterations -x 300 -y 190

# Row 6: Extra Parameters (full-width)
$lblExtra = New-Object System.Windows.Forms.Label
$lblExtra.Location = New-Object System.Drawing.Point(10,230)
$lblExtra.Size = New-Object System.Drawing.Size(120,20)
$lblExtra.Text = "Extra Params:"
$varGroup.Controls.Add($lblExtra)

$txtExtra = New-Object System.Windows.Forms.TextBox
$txtExtra.Location = New-Object System.Drawing.Point(130,230)
$txtExtra.Size = New-Object System.Drawing.Size(320,20)
$varGroup.Controls.Add($txtExtra)

# --------------------------------------------------
# Preset Selection Section (Using TreeView)
# --------------------------------------------------
$presetGroup = New-Object System.Windows.Forms.GroupBox
$presetGroup.Text = "Preset Selection"
# Place this below the Variables group.
$presetGroup.Location = New-Object System.Drawing.Point(10,440)
# Increase height to allow for the TreeView.
$presetGroup.Size = New-Object System.Drawing.Size(780,150)
$form.Controls.Add($presetGroup)

# Preset Folder label, textbox, and browse button
$lblPresetFolder = New-Object System.Windows.Forms.Label
$lblPresetFolder.Location = New-Object System.Drawing.Point(10,20)
$lblPresetFolder.Size = New-Object System.Drawing.Size(90,20)
$lblPresetFolder.Text = "Preset Folder:"
$presetGroup.Controls.Add($lblPresetFolder)

$txtPresetFolder = New-Object System.Windows.Forms.TextBox
$txtPresetFolder.Location = New-Object System.Drawing.Point(110,18)
$txtPresetFolder.Size = New-Object System.Drawing.Size(450,20)
# Default to Spooky2 preset collection folder.
$txtPresetFolder.Text = "C:\Spooky2\Preset Collections"
$presetGroup.Controls.Add($txtPresetFolder)

$btnPresetFolderBrowse = New-Object System.Windows.Forms.Button
$btnPresetFolderBrowse.Location = New-Object System.Drawing.Point(570,16)
$btnPresetFolderBrowse.Size = New-Object System.Drawing.Size(100,24)
$btnPresetFolderBrowse.Text = "Browse..."
$presetGroup.Controls.Add($btnPresetFolderBrowse)

$folderBrowserPreset = New-Object System.Windows.Forms.FolderBrowserDialog
$btnPresetFolderBrowse.Add_Click({
    if ($folderBrowserPreset.ShowDialog() -eq [System.Windows.Forms.DialogResult]::OK) {
        $txtPresetFolder.Text = $folderBrowserPreset.SelectedPath
    }
})

# TreeView to display preset files organized by subfolder.
$treeViewPresets = New-Object System.Windows.Forms.TreeView
$treeViewPresets.Location = New-Object System.Drawing.Point(10,50)
$treeViewPresets.Size = New-Object System.Drawing.Size(500,120)
$presetGroup.Controls.Add($treeViewPresets)

# Button to refresh preset tree.
$btnRefreshPresets = New-Object System.Windows.Forms.Button
$btnRefreshPresets.Location = New-Object System.Drawing.Point(520,50)
$btnRefreshPresets.Size = New-Object System.Drawing.Size(100,30)
$btnRefreshPresets.Text = "Refresh"
$presetGroup.Controls.Add($btnRefreshPresets)

# Button to load selected preset.
$btnLoadPreset = New-Object System.Windows.Forms.Button
$btnLoadPreset.Location = New-Object System.Drawing.Point(520,90)
$btnLoadPreset.Size = New-Object System.Drawing.Size(100,30)
$btnLoadPreset.Text = "Load Preset"
$presetGroup.Controls.Add($btnLoadPreset)

# Read-only textbox to display the selected preset file path (to be passed as a parameter).
$txtPreset = New-Object System.Windows.Forms.TextBox
$txtPreset.Location = New-Object System.Drawing.Point(10,135)
$txtPreset.Size = New-Object System.Drawing.Size(500,20)
$txtPreset.ReadOnly = $true
$presetGroup.Controls.Add($txtPreset)

# Function to recursively add nodes for directories and .txt files.
function Add-PresetNodes {
    param (
        [System.Windows.Forms.TreeNode]$parentNode,
        [string]$path
    )
    # Add subdirectories.
    Get-ChildItem -Path $path -Directory -ErrorAction SilentlyContinue | ForEach-Object {
        $dirNode = New-Object System.Windows.Forms.TreeNode($_.Name)
        $dirNode.Tag = $_.FullName
        $parentNode.Nodes.Add($dirNode) | Out-Null
        Add-PresetNodes -parentNode $dirNode -path $_.FullName
    }
    # Add .txt files.
    Get-ChildItem -Path $path -Filter *.txt -File -ErrorAction SilentlyContinue | ForEach-Object {
        $fileNode = New-Object System.Windows.Forms.TreeNode($_.Name)
        $fileNode.Tag = $_.FullName
        $parentNode.Nodes.Add($fileNode) | Out-Null
    }
}

# Updated Refresh-PresetTree: Instead of creating a single root node,
# add all subdirectories and .txt files in the selected folder as top-level nodes.
function Refresh-PresetTree {
    $treeViewPresets.Nodes.Clear()
    $folder = $txtPresetFolder.Text.Trim()
    if (Test-Path $folder) {
        # Add subdirectories as nodes.
        Get-ChildItem -Path $folder -Directory -ErrorAction SilentlyContinue | ForEach-Object {
            $dirNode = New-Object System.Windows.Forms.TreeNode($_.Name)
            $dirNode.Tag = $_.FullName
            $treeViewPresets.Nodes.Add($dirNode) | Out-Null
            Add-PresetNodes -parentNode $dirNode -path $_.FullName
        }
        # Add .txt files in the main folder as top-level nodes.
        Get-ChildItem -Path $folder -Filter *.txt -File -ErrorAction SilentlyContinue | ForEach-Object {
            $fileNode = New-Object System.Windows.Forms.TreeNode($_.Name)
            $fileNode.Tag = $_.FullName
            $treeViewPresets.Nodes.Add($fileNode) | Out-Null
        }
        $treeViewPresets.ExpandAll()
        $treeViewPresets.Refresh()
    }
    else {
        [System.Windows.Forms.MessageBox]::Show("Preset folder not found: $folder", "Error", [System.Windows.Forms.MessageBoxButtons]::OK, [System.Windows.Forms.MessageBoxIcon]::Error)
    }
}

$btnRefreshPresets.Add_Click({ Refresh-PresetTree })

# When a node is double-clicked, if it represents a file (i.e. a leaf node), load its full path.
$treeViewPresets.Add_NodeMouseDoubleClick({
    param($sender, $e)
    if ($e.Node.Nodes.Count -eq 0) {
        $filePath = $e.Node.Tag
        if (Test-Path $filePath) {
            $txtPreset.Text = $filePath
        }
    }
})

# Alternatively, load preset when the "Load Preset" button is clicked.
$btnLoadPreset.Add_Click({
    $selectedNode = $treeViewPresets.SelectedNode
    if ($selectedNode -and ($selectedNode.Nodes.Count -eq 0)) {
        $txtPreset.Text = $selectedNode.Tag
    }
    else {
        [System.Windows.Forms.MessageBox]::Show("No preset file selected.", "Info", [System.Windows.Forms.MessageBoxButtons]::OK, [System.Windows.Forms.MessageBoxIcon]::Information)
    }
})

# Optionally, refresh the preset tree on startup.
Refresh-PresetTree

# --------------------------------------------------
# Commands Section
# --------------------------------------------------
$cmdGroup = New-Object System.Windows.Forms.GroupBox
$cmdGroup.Text = "Commands"
# Place below the Preset section.
$cmdGroup.Location = New-Object System.Drawing.Point(10,600)
$cmdGroup.Size = New-Object System.Drawing.Size(780,70)
$form.Controls.Add($cmdGroup)

# Create buttons for each s2 command.
$commands = @("status", "scan", "pulse", "run", "set", "control")
$buttonCommands = @{}

$xPos = 10
foreach ($cmd in $commands) {
    $btn = New-Object System.Windows.Forms.Button
    $btn.Text = $cmd
    $btn.Size = New-Object System.Drawing.Size(110,30)
    $btn.Location = New-Object System.Drawing.Point($xPos, 25)
    $xPos += 120
    $cmdGroup.Controls.Add($btn)
    $buttonCommands[$cmd] = $btn
}

# --------------------------------------------------
# Output Section
# --------------------------------------------------
$lblOutput = New-Object System.Windows.Forms.Label
# Placed below the Commands section.
$lblOutput.Location = New-Object System.Drawing.Point(10,680)
$lblOutput.Size = New-Object System.Drawing.Size(100,20)
$lblOutput.Text = "Command Output:"
$form.Controls.Add($lblOutput)

$txtOutput = New-Object System.Windows.Forms.TextBox
$txtOutput.Location = New-Object System.Drawing.Point(10,705)
$txtOutput.Size = New-Object System.Drawing.Size(780,60)
$txtOutput.Multiline = $true
$txtOutput.ScrollBars = "Vertical"
$txtOutput.ReadOnly = $true
$form.Controls.Add($txtOutput)

$buttonClearOutput = New-Object System.Windows.Forms.Button
$buttonClearOutput.Location = New-Object System.Drawing.Point(690,770)
$buttonClearOutput.Size = New-Object System.Drawing.Size(100,24)
$buttonClearOutput.Text = "Clear Output"
$form.Controls.Add($buttonClearOutput)
$buttonClearOutput.Add_Click({ $txtOutput.Clear() })

# --------------------------------------------------
# Function to Build the Parameter String
# --------------------------------------------------
function Build-Parameters {
    $params = @()
    if ($txtAmplitude.Text.Trim() -ne "") { $params += "amplitude=$($txtAmplitude.Text.Trim())" }
    if ($txtChannel.Text.Trim() -ne "")   { $params += "channel=$($txtChannel.Text.Trim())" }
    if ($txtDuration.Text.Trim() -ne "")    { $params += "duration=$($txtDuration.Text.Trim())" }
    if ($txtFrequency.Text.Trim() -ne "")   { $params += "frequency=$($txtFrequency.Text.Trim())" }
    if ($cboGenerator.SelectedItem -ne $null) { $params += "generator=$($cboGenerator.SelectedItem)" }
    if ($txtPulse.Text.Trim() -ne "")       { $params += "pulse=$($txtPulse.Text.Trim())" }
    if ($cboSimulation.SelectedItem -ne $null) { $params += "simulation=$($cboSimulation.SelectedItem)" }
    if ($cboWaveform.SelectedItem -ne $null)   { $params += "waveform=$($cboWaveform.SelectedItem)" }
    if ($cboLoop.SelectedItem -ne $null)       { $params += "loop=$($cboLoop.SelectedItem)" }
    if ($txtIterations.Text.Trim() -ne "")    { $params += "iterations=$($txtIterations.Text.Trim())" }
    
    # If you need to specify the Spooky2 installation path, verify the correct path.
    # Since your s2.exe does not accept the option as used, we omit it.
    # To use it, uncomment the following block and update the path if necessary.
    # if ($textSpooky.Text.Trim() -ne "") { 
    #     $sPath = $textSpooky.Text.Trim()
    #     if ($sPath -match "\s") { $sPath = "`"$sPath`"" }
    #     $params += "spooky2=$sPath" 
    # }
    
    # Include the preset file if one has been loaded.
    if ($txtPreset.Text.Trim() -ne "") { 
        $presetPath = $txtPreset.Text.Trim()
        if ($presetPath -match "\s") { $presetPath = "`"$presetPath`"" }
        $params += "preset=$presetPath"
    }
    if ($txtExtra.Text.Trim() -ne "") { $params += $txtExtra.Text.Trim() }
    
    return $params -join " "
}

# --------------------------------------------------
# Function to Run a Command
# --------------------------------------------------
function Run-S2Command {
    param(
        [string]$cmdName
    )
    
    # Verify that the s2 executable exists.
    $exePath = $textS2.Text.Trim()
    if (-not (Test-Path $exePath)) {
        [System.Windows.Forms.MessageBox]::Show("s2 executable not found at: $exePath", "Error", [System.Windows.Forms.MessageBoxButtons]::OK, [System.Windows.Forms.MessageBoxIcon]::Error)
        return
    }
    
    # Build parameters from our fields.
    $paramString = Build-Parameters
    
    # Build the full command line (command then parameters).
    $fullArgs = "$cmdName $paramString"
    
    $txtOutput.AppendText("Running: `"$exePath`" $fullArgs" + [Environment]::NewLine)
    
    try {
        $psi = New-Object System.Diagnostics.ProcessStartInfo
        $psi.FileName = $exePath
        $psi.Arguments = $fullArgs
        $psi.RedirectStandardOutput = $true
        $psi.RedirectStandardError = $true
        $psi.UseShellExecute = $false
        $psi.CreateNoWindow = $true
    
        $proc = New-Object System.Diagnostics.Process
        $proc.StartInfo = $psi
        $proc.Start() | Out-Null
    
        $stdout = $proc.StandardOutput.ReadToEnd()
        $stderr = $proc.StandardError.ReadToEnd()
        $proc.WaitForExit()
    
        if ($stdout -ne "") {
            $txtOutput.AppendText("Output:" + [Environment]::NewLine + $stdout + [Environment]::NewLine)
        }
        if ($stderr -ne "") {
            $txtOutput.AppendText("Error:" + [Environment]::NewLine + $stderr + [Environment]::NewLine)
        }
    }
    catch {
        $txtOutput.AppendText("Exception: " + $_.Exception.Message + [Environment]::NewLine)
    }
}

# --------------------------------------------------
# Attach Event Handlers to Command Buttons
# --------------------------------------------------
foreach ($cmd in $commands) {
    $buttonCommands[$cmd].Add_Click({ Run-S2Command -cmdName $cmd })
}

# --------------------------------------------------
# Show the Form
# --------------------------------------------------
[void] $form.ShowDialog()
