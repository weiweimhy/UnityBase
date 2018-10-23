# Unity 基础工程

## 功能点
- Log系统
- Xcode导出
- Extension扩展


## Release notes
### v0.0.2
- 增加Unity查看Android日志的工具，Window -> Logcat
- 增加大量扩展函数
- 整理优化Log系统

### v0.0.1
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
    - 修改plist.info，目前只支持string-string
    - 添加File
