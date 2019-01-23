//
//  Bugly_iOS.m
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <Bugly/Bugly.h>
#import "BuglyAttachCollector.h"
#import "BuglyAgent.h"

extern "C" {
    void bugly_init(const char * key, bool debug, int reportLevel, bool openBlock, float blockTime) {
        NSString *nsKey = [NSString stringWithCString:key encoding:NSStringEncodingConversionAllowLossy];
        assert(nsKey.length > 0);
        
        BuglyConfig *bglConfig = [BuglyConfig new];
        bglConfig.reportLogLevel = (BuglyLogLevel)reportLevel;
        bglConfig.blockMonitorEnable = openBlock;
        bglConfig.blockMonitorTimeout = blockTime;
        bglConfig.debugMode = debug;
        bglConfig.unexpectedTerminatingDetectionEnable = YES;
        bglConfig.delegate = [BuglyAgent shared];
        [Bugly startWithAppId:nsKey config:bglConfig];
    }
    
    void bugly_v(const char * msg) {
        NSString *nsMsg = [NSString stringWithCString:msg encoding:NSStringEncodingConversionAllowLossy];
        assert(nsMsg.length > 0);
        
        BLYLogVerbose(@"%@", nsMsg);
    }
    
    void bugly_d(const char * msg) {
        NSString *nsMsg = [NSString stringWithCString:msg encoding:NSStringEncodingConversionAllowLossy];
        assert(nsMsg.length > 0);
        
        BLYLogDebug(@"%@", nsMsg);
    }
    
    void bugly_i(const char * msg) {
        NSString *nsMsg = [NSString stringWithCString:msg encoding:NSStringEncodingConversionAllowLossy];
        assert(nsMsg.length > 0);
        
        BLYLogInfo(@"%@", nsMsg);
    }
    
    void bugly_w(const char * msg) {
        NSString *nsMsg = [NSString stringWithCString:msg encoding:NSStringEncodingConversionAllowLossy];
        assert(nsMsg.length > 0);
        
        BLYLogWarn(@"%@", nsMsg);
    }
    
    void bugly_e(const char * msg) {
        NSString *nsMsg = [NSString stringWithCString:msg encoding:NSStringEncodingConversionAllowLossy];
        assert(nsMsg.length > 0);
        
        BLYLogError(@"%@", nsMsg);
    }
    
    void bugly_setUserId(const char * userId) {
        NSString *nsUserId = [NSString stringWithCString:userId encoding:NSStringEncodingConversionAllowLossy];
        assert(nsUserId.length > 0);
        
        [Bugly setUserIdentifier:nsUserId];
    }
    
    void bugly_putUserData(const char * key, const char * value) {
        NSString *nsKey = [NSString stringWithCString:key encoding:NSStringEncodingConversionAllowLossy];
        assert(nsKey.length > 0);
        NSString *nsValue = [NSString stringWithCString:value encoding:NSStringEncodingConversionAllowLossy];
        assert(nsValue.length > 0);
        
        [Bugly setUserValue:nsValue forKey:nsKey];
    }
    
    void bugly_setUserSceneTag(int tagId) {
        [Bugly setTag:tagId];
    }
    
    void bugly_addExtraData(const char * msg) {
        NSString *nsMsg = [NSString stringWithCString:msg encoding:NSStringEncodingConversionAllowLossy];
        [[BuglyAttachCollector shared] appendAttachMessage:nsMsg];
    }

    void bugly_reportException(const char * name, const char * reason, const char * info) {
        NSString *nsName = [NSString stringWithCString:name encoding:NSStringEncodingConversionAllowLossy];
        assert(nsName.length > 0);
        NSString *nsReason = [NSString stringWithCString:reason encoding:NSStringEncodingConversionAllowLossy];
        assert(nsReason.length > 0);
        NSString *nsInfo = [NSString stringWithCString:info encoding:NSStringEncodingConversionAllowLossy];
        assert(nsInfo != nil);
        [Bugly reportExceptionWithCategory:4 name:nsName reason:nsReason callStack:@[nsInfo] extraInfo:@{} terminateApp:NO];
    }

    void bugly_testCrash() {
#if DEBUG
        NSException *exception = [NSException exceptionWithName:@"Bugly TestException"
                                                         reason:@"This is an Objective-C exception using for testing."
                                                       userInfo:nil];
        [exception raise];
#endif
    }
}
