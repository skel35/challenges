#include <iostream>
#include <fstream>
#include <algorithm>
#include <utility>
#define ll long long
#define uchar unsigned char
using namespace std;
const ll p = 1e9 + 7;
bool pairCompare(const pair<long, bool>& firstElem, const pair<double, bool>& secondElem) {
	return firstElem.first > secondElem.first;
}

int main() {
	ifstream cin("input.txt");
	ofstream cout("output.txt");
	uchar t;
	cin >> t;
	for (int i = 0; i < t; i++) {
		long n, m;
		cin >> m >> n;
		pair<long, bool>* c = new pair<long, bool>[m + n - 2];
		// (x, true)
		// (y, false)
		long temp;
		for (int j = 0; j < m - 1; j++) {
			cin >> temp;
			c[j] = make_pair(temp, true);
		}
		for (int j = 0; j < n - 1; j++) {
			cin >> temp;
			c[m - 1 + j] = make_pair(temp, false);
		}
		sort(c, c + m + n - 2, pairCompare);
		long nx = 1, ny = 1;
		ll sum = 0;
		for (int j = 0; j < m + n - 2; j++) {
			if (c[j].second) {	// x
				sum = (sum + (c[j].first * ny) % p) % p;
				nx++;
			}
			else {	// y
				sum = (sum + (c[j].first * nx) % p) % p;
				ny++;
			}
		}
		cout << sum << endl;
	}

}