//
//  iOSLog.m
//  Unity-iPhone
//
//  Created by mhy on 2018/10/17.
//

#import <CocoaLumberjack/CocoaLumberjack.h>

static const int ddLogLevel = DDLogLevelVerbose;

bool initFinished = false;

void initLog() {
    if(initFinished) return;
    
#if TARGET_IPHONE_SIMULATOR
    [DDLog addLogger:[DDTTYLogger new]];
#else
    [DDLog addLogger:[DDASLLogger new]];
#endif
    initFinished = true;
}

void v(char * msg) {
    initLog();
    DDLogVerbose([NSString stringWithUTF8String:msg]);
}

void d(char * msg) {
    initLog();
    DDLogDebug([NSString stringWithUTF8String:msg]);
}

void i(char * msg) {
    initLog();
    DDLogInfo([NSString stringWithUTF8String:msg]);
}

void w(char * msg) {
    initLog();
    DDLogWarn([NSString stringWithUTF8String:msg]);
}

void e(char * msg) {
    initLog();
    DDLogError([NSString stringWithUTF8String:msg]);
}
