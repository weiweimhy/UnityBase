//
//  JSONHelper.m
//  UnityCommonBridges
//
//  Created by leon on 2019/1/11.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import "JSONHelper.h"

@implementation JSONHelper

+ (nullable NSDictionary *)convertNString2Dictionary:(NSString *)json {
    NSData *nsJSONData = [json dataUsingEncoding:NSUTF8StringEncoding allowLossyConversion:YES];
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

+ (nullable NSDictionary *)convertCString2Dictionary:(const char *)json {
    NSString *nsJSONString = [NSString stringWithCString:json encoding:NSStringEncodingConversionAllowLossy];
    if (nsJSONString.length <= 0) {
        return nil;
    }

    return [self convertNString2Dictionary:nsJSONString];
}

+ (nullable NSString *)convertDictionary2NSString:(NSDictionary *)dictionary {
    if (!dictionary) {
        return nil;
    }
    
    NSError *error = nil;
    NSData *nsJSONData = [NSJSONSerialization dataWithJSONObject:dictionary options:NSJSONWritingPrettyPrinted error:&error];
    if (nsJSONData.length <= 0) {
        return nil;
    }
    
    NSString *nsJSONString = [[NSString alloc] initWithData:nsJSONData encoding:NSUTF8StringEncoding];
    if (nsJSONString.length <= 0) {
        return nil;
    }
    
    return nsJSONString;
}

+ (nullable const char *)convertDictionary2CString:(NSDictionary *)dictionary {
    NSString *nsJSONString = [self convertDictionary2NSString:dictionary];
    if (nsJSONString.length <= 0) {
        return NULL;
    }
    
    const char *cString = [nsJSONString cStringUsingEncoding:NSStringEncodingConversionAllowLossy];
    return cString;
}

@end
