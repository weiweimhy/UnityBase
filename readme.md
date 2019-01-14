# Unity 基础工程

## 功能点
- Log系统
- Xcode/Android Studio导出
- Extension扩展
- 单例和对象池
- Task模块
- Res模块
- 常用第三方库
  * firebase
  * bugly
  * appsflyer
- 屏幕适配
- Crash捕获
- 命令行工具
- Lunch场景管理
- 常用UI
- 场景切换动画

## Release notes
### v1.0.0
- 重构大量代码
- 删除Google resolver
- 增加屏幕适配管理
- 接入常用第三方库
- 增加DOTween动画
- 增加splash和场景切换动画

### v0.0.6
- 新增Res模块
- 重构CoroutineTask

### v0.05
- 新增MonoPool
- 重构部分Task模块

### v0.0.4
- 优化单例模块
- 优化对象池模块
- 增加task模块
  * SimpleTask简单的Task，同步调用
  * CoroutineTask用Coroutine实现，依赖MonoBehaviour，在场景切换以后停止执行，并自动回收
  * Runable用Thread实现，在场景切换以后继续执行，OnStart在调用线程执行，Do/OnFinish/OnCancle在新启线程执行，**不要在其他线程操作Unity相关**

### v0.0.3
- 增加单例模块
- 增加对象池模块

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
