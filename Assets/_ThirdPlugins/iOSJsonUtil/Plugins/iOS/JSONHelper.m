//
//  JSONHelper.m
//  UnityCommonBridges
//
//  Created by leon on 2019/1/11.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import "JSONHelper.h"

@implementation JSONHelper

+ (nullable NSDictionary *)convert2Dictionary:(const char *)json {
    NSString *nsJSONString = [NSString stringWithCString:json encoding:NSStringEncodingConversionAllowLossy];
    if (nsJSONString.length <= 0) {
        return nil;
    }

    NSData *nsJSONData = [nsJSONString dataUsingEncoding:NSUTF8StringEncoding allowLossyConversion:YES];
    if (nsJSONData.length <= 0) {
        return nil;
    }
    
    NSError *error = nil;
    id JSONObject = [NSJSONSerialization JSONObjectWithData:nsJSONData options:NSJSONReadingAllowFragments error:&error];
    if (![JSONObject isKindOfClass:[NSDictionary class]]) {
        return nil;
    }

    return JSONObject;
}

+ (nullable const char *)convert2CString:(NSDictionary *)dictionary {
    if (!dictionary) {
        return NULL;
    }
    
    NSError *error = nil;
    NSData *nsJSONData = [NSJSONSerialization dataWithJSONObject:dictionary options:NSJSONWritingPrettyPrinted error:&error];
    if (nsJSONData.length <= 0) {
        return NULL;
    }

    NSString *nsJSONString = [[NSString alloc] initWithData:nsJSONData encoding:NSUTF8StringEncoding];
    if (nsJSONString.length <= 0) {
        return NULL;
    }
    
    const char *cString = [nsJSONString cStringUsingEncoding:NSStringEncodingConversionAllowLossy];
    return cString;
}

@end
