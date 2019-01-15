//
//  FirebaseCrashlytics_iOS.m
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <Crashlytics/Crashlytics.h>

extern "C" {
    void fbc_testCrash() {
        [[Crashlytics sharedInstance] crash];
    }
    
    void fbc_logEvent(const char * eventName) {
        NSString *nsEventName = [NSString stringWithCString:eventName encoding:NSStringEncodingConversionAllowLossy];
        if (nsEventName.length <= 0) {
            return;
        }
        
        [Answers logCustomEventWithName:nsEventName customAttributes:nil];
    }
    
    void fbc_setString(const char * key, const char * value) {
        NSString *nsKey = [NSString stringWithCString:key encoding:NSStringEncodingConversionAllowLossy];
        assert(nsKey.length > 0);
        NSString *nsValue = [NSString stringWithCString:value encoding:NSStringEncodingConversionAllowLossy];
        assert(nsValue.length > 0);

        [[Crashlytics sharedInstance] setObjectValue:nsValue forKey:nsKey];
    }
    
    void fbc_setBool(const char * key, bool value) {
        NSString *nsKey = [NSString stringWithCString:key encoding:NSStringEncodingConversionAllowLossy];
        assert(nsKey.length > 0);

        [[Crashlytics sharedInstance] setBoolValue:value forKey:nsKey];
    }
    
    void fbc_setFloat(const char * key, float value) {
        NSString *nsKey = [NSString stringWithCString:key encoding:NSStringEncodingConversionAllowLossy];
        assert(nsKey.length > 0);
        
        [[Crashlytics sharedInstance] setFloatValue:value forKey:nsKey];
    }
    
    void fbc_setInt(const char * key, int value) {
        NSString *nsKey = [NSString stringWithCString:key encoding:NSStringEncodingConversionAllowLossy];
        assert(nsKey.length > 0);
        
        [[Crashlytics sharedInstance] setIntValue:value forKey:nsKey];
    }
    
    void fbc_setUserIdentifier(const char * identifier) {
        NSString *nsIdentifier = [NSString stringWithCString:identifier encoding:NSStringEncodingConversionAllowLossy];
        assert(nsIdentifier.length > 0);
        
        [[Crashlytics sharedInstance] setUserIdentifier:nsIdentifier];
    }    
}