# Unity 基础工程

## v0.0.1
- 增加基础Log系统，Android/iOS使用不同的Log系统
  * Android Log.*
  * iOS CocoaLumberjack库
- 增加Xcode打包脚本及打包配置文件
  * 配置文件：Build -> Edit Build Project Setting
  * Xcode打包脚本支持：
    - 设置自动签名
    - 开启系统Capability,Eg: IAP
    - 添加Framework
    - 添加Library
    - 修改属性，Eg：关闭bitcode，添加other link flag
    - 修改plist.info，目前只支持stirng-string
