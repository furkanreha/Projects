#include <iostream>
#include <string>
#include <iomanip>
#include <cstdio>
#include "run-solutions.h"
using namespace std;

void printStar() {
    printf("*****************************************************************\n");
}

/* ----------------------------Solutions Start---------------------------- */

void helloWorld() {
    cout << "Hello, World!" << endl;
}

void inputOutput() {
    int input, sum = 0;
    for (int i = 0; i < 3; i++)
    {
        cin >> input;
        sum += input;
    }
    cout << sum;
}

void basicDataTypes() {
    int a;
    long b;
    char c;
    float d;
    double e;

    cin >> a >> b >> c >> d >> e;
    cout << a << endl << b << endl << c << endl << setprecision(3) << fixed << d << endl << setprecision(9) << fixed << e;
}

void conditionalStatements() {
    int n;
    cin >> n;

    if (n > 9)
    {
        printf("Greater than 9");
    }
    else if (n == 1)
    {
        printf("one");
    }

    else if (n == 2)
    {   
        printf("two");
    }

    else if (n == 3)
    {   
        printf("three");
    }

    else if (n == 4)
    {   
        printf("four");
    }
    else if (n == 5)
    {   
        printf("five");
    }
    else if (n == 6)
    {   
        printf("six");
    }
    else if (n == 7)
    {
        printf("seven");
    }
    else if (n == 8)
    {
        printf("eight");
    }
    else if (n == 9)
    {
        printf("nine");
    }
}

void forLoop() {
    int a, b;
    cin >> a >> b;
    string s[9] = { "one", "two", "three", "four", "five", "six","seven", "eight","nine" };

    for (int i = a; i <= b; i++) {
        if (i > 0 && i < 10) {
            cout << s[i - 1];
        }
        else {
            if (i % 2 == 0) {
                printf("even");
            }
            else {
                printf("odd");
            }
        }
    }
}

int max_of_four(int a, int b, int c, int d) {
    int max = a;
    if (max < b) {
        max = b;
    }

    if (max < c) {
        max = c;
    }

    if (max < d) {
        max = d;
    }

    return max;
}

void functions() {
    int a, b, c, d;
    cin >> a >> b >> c >> d;
    int ans = max_of_four(a, b, c, d);
    printf("%d", ans);
}

void updatePointerValues(int* a, int* b) {
    int i = (*a - *b);
    *a = *a + *b;
    *b = abs(i);
}

void pointer() {
    int a, b;
    int* pa = &a, * pb = &b;

    cin >> a >> b; 
    updatePointerValues(pa, pb);
    printf("%d\n%d", a, b);
}

void arraysIntroduction() {
    int N, input;
    cin >> N;
    int* data = nullptr;
    if (N > 0 && N < 1001) {
        data = new int[N]();
        for (int i = 0; i < N; i++) {
            cin >> input;
            if (input > 0 && input < 10001)
            {
                data[i] = input;
            }
        }

        for (int i = N - 1; i >= 0; i--) {
            cout << data[i] << " ";
        }
        cout << endl;
        delete[] data;
    }
}

/* ----------------------------Solutions End---------------------------- */

int main() {
    printStar();
    printf("This program contains solutions to HackerRank C++ Questions..\n");
    printStar();

    arraysIntroduction();
    return 0;
}