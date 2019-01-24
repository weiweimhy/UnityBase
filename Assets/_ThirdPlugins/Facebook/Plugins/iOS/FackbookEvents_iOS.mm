//
//  FackbookEvents_iOS.m
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <FBSDKCoreKit/FBSDKCoreKit.h>
#import "JSONHelper.h"

extern "C" {
    void fb_logEvent(const char * eventName, double value, const char * json) {
        NSString *nsEventName = [NSString stringWithCString:eventName encoding:NSStringEncodingConversionAllowLossy];
        assert(nsEventName.length > 0);
        NSDictionary *JSONValue = [JSONHelper convertCString2Dictionary:json];
        [FBSDKAppEvents logEvent:nsEventName valueToSum:value parameters:JSONValue];
    }
}
