# WiFi连接检测项目 - 完整文件结构

## 📁 项目文件夹结构

```
WiFiDetector/
│
├── WiFiDetector.sln                  # 解决方案文件
│
└── WiFiDetector/                     # 项目文件夹
    ├── WiFiDetector.csproj           # 项目文件
    ├── App.config                    # 应用配置文件
    ├── Program.cs                    # 程序入口点
    ├── MainForm.cs                   # 主窗体代码
    ├── MainForm.Designer.cs          # 窗体设计器代码
    ├── MainForm.resx                 # 窗体资源文件
    │
    ├── Properties/                   # 项目属性文件夹
    │   ├── AssemblyInfo.cs          # 程序集信息
    │   ├── Resources.resx           # 资源文件
    │   ├── Resources.Designer.cs    # 资源设计器
    │   └── Settings.settings        # 设置文件
    │
    ├── bin/                          # 编译输出目录
    └── obj/                          # 临时对象目录
```

---

## 📄 文件内容

### 1️⃣ **WiFiDetector.csproj** (项目文件)

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{YOUR-GUID-HERE}</ProjectGuid>
    <OutputType>WinExe</OutputType>
    <RootNamespace>WiFiDetector</RootNamespace>
    <AssemblyName>WiFiDetector</AssemblyName>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
    <Deterministic>true</Deterministic>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Core" />
    <Reference Include="System.Xml.Linq" />
    <Reference Include="System.Data.DataSetExtensions" />
    <Reference Include="Microsoft.CSharp" />
    <Reference Include="System.Data" />
    <Reference Include="System.Deployment" />
    <Reference Include="System.Drawing" />
    <Reference Include="System.Net.Http" />
    <Reference Include="System.Windows.Forms" />
    <Reference Include="System.Xml" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="MainForm.cs">
      <SubType>Form</SubType>
    </Compile>
    <Compile Include="MainForm.Designer.cs">
      <DependentUpon>MainForm.cs</DependentUpon>
    </Compile>
    <Compile Include="Program.cs" />
    <Compile Include="Properties\AssemblyInfo.cs" />
    <EmbeddedResource Include="MainForm.resx">
      <DependentUpon>MainForm.cs</DependentUpon>
    </EmbeddedResource>
    <EmbeddedResource Include="Properties\Resources.resx">
      <Generator>ResXFileCodeGenerator</Generator>
      <LastGenOutput>Resources.Designer.cs</LastGenOutput>
      <SubType>Designer</SubType>
    </EmbeddedResource>
    <Compile Include="Properties\Resources.Designer.cs">
      <AutoGen>True</AutoGen>
      <DependentUpon>Resources.resx</DependentUpon>
    </Compile>
    <None Include="Properties\Settings.settings">
      <Generator>SettingsSingleFileGenerator</Generator>
      <LastGenOutput>Settings.Designer.cs</LastGenOutput>
    </None>
    <Compile Include="Properties\Settings.Designer.cs">
      <AutoGen>True</AutoGen>
      <DependentUpon>Settings.settings</DependentUpon>
      <DesignTimeSharedInput>True</DesignTimeSharedInput>
    </Compile>
  </ItemGroup>
  <ItemGroup>
    <None Include="App.config" />
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
</Project>
```

---

### 2️⃣ **Program.cs** (程序入口)

```csharp
using System;
using System.Windows.Forms;

namespace WiFiDetector
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
```

---

### 3️⃣ **MainForm.cs** (主窗体逻辑)

```csharp
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace WiFiDetector
{
    public partial class MainForm : Form
    {
        #region 方案1：使用 NetworkInterface (简单方法)
        
        /// <summary>
        /// 检测WiFi是否连接 - 简单方法
        /// </summary>
        private bool IsWiFiConnected_Simple()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // 检查是否为无线网卡且状态为已连接
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                    ni.OperationalStatus == OperationalStatus.Up)
                {
                    return true;
                }
            }
            return false;
        }
        
        #endregion

        #region 方案2：使用 Windows Native WiFi API (高级方法)

        // 导入 wlanapi.dll 函数
        [DllImport("wlanapi.dll")]
        public static extern int WlanOpenHandle(
            uint dwClientVersion,
            IntPtr pReserved,
            out uint pdwNegotiatedVersion,
            out IntPtr phClientHandle);

        [DllImport("wlanapi.dll")]
        public static extern int WlanCloseHandle(
            IntPtr hClientHandle,
            IntPtr pReserved);

        [DllImport("wlanapi.dll")]
        public static extern int WlanEnumInterfaces(
            IntPtr hClientHandle,
            IntPtr pReserved,
            out IntPtr ppInterfaceList);

        [DllImport("wlanapi.dll")]
        public static extern void WlanFreeMemory(IntPtr pMemory);

        // WLAN 接口信息结构
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WLAN_INTERFACE_INFO
        {
            public Guid InterfaceGuid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string strInterfaceDescription;
            public WLAN_INTERFACE_STATE isState;
        }

        // WLAN 接口列表结构
        [StructLayout(LayoutKind.Sequential)]
        public struct WLAN_INTERFACE_INFO_LIST
        {
            public uint dwNumberOfItems;
            public uint dwIndex;
            public WLAN_INTERFACE_INFO[] InterfaceInfo;
        }

        // WLAN 接口状态枚举
        public enum WLAN_INTERFACE_STATE
        {
            wlan_interface_state_not_ready = 0,
            wlan_interface_state_connected = 1,
            wlan_interface_state_ad_hoc_network_formed = 2,
            wlan_interface_state_disconnecting = 3,
            wlan_interface_state_disconnected = 4,
            wlan_interface_state_associating = 5,
            wlan_interface_state_discovering = 6,
            wlan_interface_state_authenticating = 7
        }

        /// <summary>
        /// 检测WiFi是否连接 - 高级方法
        /// </summary>
        private bool IsWiFiConnected_Advanced()
        {
            IntPtr clientHandle = IntPtr.Zero;
            uint negotiatedVersion;

            try
            {
                // 打开WLAN句柄
                int result = WlanOpenHandle(2, IntPtr.Zero, out negotiatedVersion, out clientHandle);
                if (result != 0)
                {
                    return false;
                }

                // 枚举WLAN接口
                IntPtr interfaceListPtr;
                result = WlanEnumInterfaces(clientHandle, IntPtr.Zero, out interfaceListPtr);
                if (result != 0)
                {
                    return false;
                }

                try
                {
                    // 获取接口列表头部
                    WLAN_INTERFACE_INFO_LIST interfaceList = 
                        (WLAN_INTERFACE_INFO_LIST)Marshal.PtrToStructure(
                            interfaceListPtr, 
                            typeof(WLAN_INTERFACE_INFO_LIST));

                    // 计算接口信息的大小和偏移
                    int infoSize = Marshal.SizeOf(typeof(WLAN_INTERFACE_INFO));
                    IntPtr currentPtr = new IntPtr(interfaceListPtr.ToInt64() + 8);

                    // 遍历所有WLAN接口
                    for (int i = 0; i < interfaceList.dwNumberOfItems; i++)
                    {
                        WLAN_INTERFACE_INFO info = 
                            (WLAN_INTERFACE_INFO)Marshal.PtrToStructure(
                                currentPtr, 
                                typeof(WLAN_INTERFACE_INFO));

                        // 检查接口是否已连接
                        if (info.isState == WLAN_INTERFACE_STATE.wlan_interface_state_connected)
                        {
                            return true;
                        }

                        currentPtr = new IntPtr(currentPtr.ToInt64() + infoSize);
                    }
                }
                finally
                {
                    WlanFreeMemory(interfaceListPtr);
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (clientHandle != IntPtr.Zero)
                {
                    WlanCloseHandle(clientHandle, IntPtr.Zero);
                }
            }

            return false;
        }

        #endregion

        #region 窗体初始化和检测逻辑

        public MainForm()
        {
            InitializeComponent();
            
            // 窗体加载后立即检测WiFi连接
            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckWiFiConnection();
        }

        /// <summary>
        /// 检测WiFi连接状态并显示提示
        /// </summary>
        private void CheckWiFiConnection()
        {
            // 使用方案1（推荐，简单可靠）
            bool isConnected = IsWiFiConnected_Simple();

            // 或使用方案2（更精确，但代码更复杂）
            // bool isConnected = IsWiFiConnected_Advanced();

            if (!isConnected)
            {
                MessageBox.Show(
                    "未检测到WiFi连接！\n\n请检查您的无线网络连接。",
                    "WiFi连接提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            // 如果已连接WiFi，什么也不做（静默）
        }

        #endregion
    }
}
```

---

### 4️⃣ **MainForm.Designer.cs** (窗体设计器代码)

```csharp
namespace WiFiDetector
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelInfo = new System.Windows.Forms.Label();
            this.btnRecheck = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInfo.Location = new System.Drawing.Point(80, 80);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(242, 21);
            this.labelInfo.TabIndex = 0;
            this.labelInfo.Text = "程序已启动，WiFi检测已完成";
            // 
            // btnRecheck
            // 
            this.btnRecheck.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnRecheck.Location = new System.Drawing.Point(140, 150);
            this.btnRecheck.Name = "btnRecheck";
            this.btnRecheck.Size = new System.Drawing.Size(120, 35);
            this.btnRecheck.TabIndex = 1;
            this.btnRecheck.Text = "重新检测";
            this.btnRecheck.UseVisualStyleBackColor = true;
            this.btnRecheck.Click += new System.EventHandler(this.btnRecheck_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 250);
            this.Controls.Add(this.btnRecheck);
            this.Controls.Add(this.labelInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WiFi连接检测程序";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button btnRecheck;

        // 重新检测按钮点击事件
        private void btnRecheck_Click(object sender, System.EventArgs e)
        {
            CheckWiFiConnection();
        }
    }
}
```

---

### 5️⃣ **MainForm.resx** (窗体资源文件)

```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:import namespace="http://www.w3.org/XML/1998/namespace" />
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" use="required" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
</root>
```

---

### 6️⃣ **App.config** (应用配置文件)

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
    </startup>
</configuration>
```

---

### 7️⃣ **Properties/AssemblyInfo.cs** (程序集信息)

```csharp
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的一般信息由以下
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("WiFiDetector")]
[assembly: AssemblyDescription("WiFi连接检测工具")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("WiFiDetector")]
[assembly: AssemblyCopyright("Copyright ©  2024")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 会使此程序集中的类型
//对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("12345678-1234-1234-1234-123456789abc")]

// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
//可以指定所有这些值，也可以使用"生成号"和"修订号"的默认值
//通过使用 "*"，如下所示:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
```

---

## 🚀 快速创建步骤

### 方法一：使用 Visual Studio（推荐）

1. **新建项目**
   - 打开 Visual Studio
   - 文件 → 新建 → 项目
   - 选择"Windows 窗体应用(.NET Framework)"
   - 项目名称：`WiFiDetector`
   - 框架：`.NET Framework 4.7.2`

2. **替换文件内容**
   - 将 `Program.cs` 替换为上面的内容
   - 将 `Form1.cs` 重命名为 `MainForm.cs`
   - 将 `MainForm.cs`、`MainForm.Designer.cs` 替换为上面的内容

3. **编译运行**
   - 按 `F5` 或点击"启动"按钮

### 方法二：手动创建文件

1. 创建项目文件夹结构
2. 复制上述所有文件内容
3. 使用 MSBuild 或 Visual Studio 编译

---

## 📌 注意事项

- **GUID 生成**：项目文件中的 `{YOUR-GUID-HERE}` 需要替换为实际的 GUID
- **引用完整性**：确保所有 `System.Windows.Forms` 和 `System.Net.NetworkInformation` 引用已添加
- **资源文件**：`Properties` 文件夹中的资源文件由 Visual Studio 自动生成
- **权限**：程序需要访问网络接口信息的权限（一般无需特殊配置）

---

## ✨ 功能特性

✅ 启动时自动检测 WiFi 连接  
✅ 提供简单方法和高级方法两种检测方案  
✅ 包含"重新检测"按钮，可手动触发检测  
✅ 友好的用户界面和提示信息  
✅ 完整的项目结构，便于扩展