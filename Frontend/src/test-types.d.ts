// Tipos globais para testes com Jasmine

/// <reference types="jasmine" />

declare namespace jasmine {
  interface Matchers<T> {
    toBeTruthy(): boolean;
    toBeFalsy(): boolean;
    toBeNull(): boolean;
    toBeUndefined(): boolean;
    toBeDefined(): boolean;
    toBeTrue(): boolean;
    toBeFalse(): boolean;
    toEqual(expected: any): boolean;
    toBe(expected: any): boolean;
    toContain(expected: any): boolean;
    toHaveBeenCalled(): boolean;
    toHaveBeenCalledWith(...args: any[]): boolean;
    toHaveBeenCalledTimes(expected: number): boolean;
  }

  interface SpyObj<T> {
    [K in keyof T]: T[K] extends (...args: any[]) => any ? jasmine.Spy<T[K]> : T[K];
  }

  function createSpyObj<T>(baseName: string, methodNames: (keyof T)[]): SpyObj<T>;
  function createSpyObj<T>(baseName: string, methodNames: any): SpyObj<T>;
}

declare function describe(description: string, specDefinitions: () => void): void;
declare function it(expectation: string, assertion?: (done?: DoneFn) => void, timeout?: number): void;
declare function beforeEach(action: (done?: DoneFn) => void, timeout?: number): void;
declare function afterEach(action: (done?: DoneFn) => void, timeout?: number): void;
declare function beforeAll(action: (done?: DoneFn) => void, timeout?: number): void;
declare function afterAll(action: (done?: DoneFn) => void, timeout?: number): void;
declare function expect<T>(actual: T): jasmine.Matchers<T>;
declare function fail(e?: any): void;
declare function pending(reason?: string): void;
declare function spyOn<T>(object: T, method: keyof T): jasmine.Spy;
declare function spyOnProperty<T>(object: T, property: keyof T, accessType?: 'get' | 'set'): jasmine.Spy;

interface DoneFn {
  (): void;
  fail: (message?: Error | string) => void;
}