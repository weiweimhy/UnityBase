//
//  FirebaseAnalytics_iOS.m
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <FirebaseAnalytics/FirebaseAnalytics.h>
#import <FirebaseCore/FirebaseCore.h>

extern "C" {
    void fba_init(){
        [FIRApp configure];
    }
    
    void fba_logEvent(const char * eventName) {
        NSString *nsEventName = [NSString stringWithCString:eventName encoding:NSStringEncodingConversionAllowLossy];
        assert(nsEventName.length > 0);
        [FIRAnalytics logEventWithName:nsEventName parameters:nil];
    }
    
    void fba_logEventString(const char *  eventName, const char *  key, const char *  value) {
        NSString *nsEventName = [NSString stringWithCString:eventName encoding:NSStringEncodingConversionAllowLossy];
        assert(nsEventName.length > 0);
        NSString *nsKey = [NSString stringWithCString:key encoding:NSStringEncodingConversionAllowLossy];
        assert(nsKey.length > 0);
        NSString *nsValue = [NSString stringWithCString:value encoding:NSStringEncodingConversionAllowLossy];
        assert(nsValue.length > 0);
        
        [FIRAnalytics logEventWithName:nsEventName parameters:@{nsKey : nsValue}];
    }
    
    void fba_logEventInt(const char *  eventName, const char *  key, int value) {
        NSString *nsEventName = [NSString stringWithCString:eventName encoding:NSStringEncodingConversionAllowLossy];
        assert(nsEventName.length > 0);
        NSString *nsKey = [NSString stringWithCString:key encoding:NSStringEncodingConversionAllowLossy];
        assert(nsKey.length > 0);
        
        [FIRAnalytics logEventWithName:nsEventName parameters:@{nsKey : @(value)}];
    }
    
    void fba_logEventFloat(const char *  eventName, const char *  key, float value) {
        NSString *nsEventName = [NSString stringWithCString:eventName encoding:NSStringEncodingConversionAllowLossy];
        assert(nsEventName.length > 0);
        NSString *nsKey = [NSString stringWithCString:key encoding:NSStringEncodingConversionAllowLossy];
        assert(nsKey.length > 0);
        
        [FIRAnalytics logEventWithName:nsEventName parameters:@{nsKey : @(value)}];
    }
    
    void fba_logEventLong(const char *  eventName, const char *  key, long value) {
        NSString *nsEventName = [NSString stringWithCString:eventName encoding:NSStringEncodingConversionAllowLossy];
        assert(nsEventName.length > 0);
        NSString *nsKey = [NSString stringWithCString:key encoding:NSStringEncodingConversionAllowLossy];
        assert(nsKey.length > 0);
        
        [FIRAnalytics logEventWithName:nsEventName parameters:@{nsKey : @(value)}];
    }
}