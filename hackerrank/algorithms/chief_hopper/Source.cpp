#include <algorithm>
#include <cmath>
#include <fstream>
#include <iostream>
#include <utility>
#include <vector>
#define ll long long
#define uchar unsigned char
using namespace std;

int main() {
	ifstream cin("input.txt");
	ofstream cout("output.txt");
	ll n;
	cin >> n;
	int* h = new int[n];
	const int divisor = 30;
	int* maxh = new int[n / divisor + 1];

	for (int i = 0; i < n / divisor; i++) {
		maxh[i] = 0;
		for (int j = 0; j < divisor; j++) {
			cin >> h[i * divisor + j];
			if (h[i * divisor + j] > maxh[i]) maxh[i] = h[i * divisor + j];
		}
	}
	maxh[n / divisor] = 0;
	for (int i = 0; i < n % divisor; i++) {
		cin >> h[n - (n%divisor) + i];
		if (h[n - (n % divisor) + i] > maxh[n / divisor]) {
			maxh[n / divisor] = h[n - (n % divisor) + i];
		}
	}

	ll ans = 0;

	// dividing n into (n/divisor) + (n%divisor > 0) parts
	// going through parts starting from the last one

	if (n % divisor) {
		unsigned ll energy = maxh[n / divisor];
		for (int i = 0; i < n % divisor; i++) {
			energy += energy - h[n - (n % divisor) + i];
		}
		ans = maxh[n / divisor] - (energy >> (n%divisor));

	}
	
	for (int i = n / divisor - 1; i >= 0; i--) {
		unsigned ll energy = maxh[i];
		for (int j = 0; j < divisor; j++) {
			energy += energy - h[i * divisor + j];
		}
		ans = maxh[i] - ((energy - ans) >> divisor);
	}

	cout << ans;

}