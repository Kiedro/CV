#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include "inc\helper_cuda.h"
#include <stdio.h>
#include <iostream>
#include <fstream>
#include <cstdlib>
#include <string>
#include <algorithm>
#include <ctime>
#include <conio.h>

#include <tchar.h>
#include <windows.h>
#include "cuda_runtime.h"
#include "device_launch_parameters.h"

#include <thrust/sort.h>

#include <stdio.h>
using namespace std;

///// DEFINE ///////
#define numberOfCities 100
const char * fileName = "kroa100.txt";
#define populationSize  4000          // wielkoœæ musi byæ wielokrotnoœci¹ 4, inaczej krzy¿owanie nie dzia³a do koñca poprawnie
#define iterations 10000

////// zmienne globalne ////////////////////////////////////////
int citiesArray[numberOfCities + 1][3];                       // miasta odczytane z pliku
int citiesDistance[(numberOfCities + 1)*(numberOfCities)];    // odleg³osci pomiedzy miastami 1D, +1 bo miasto o nr x ma miejsce w tablicy x
int populationArray[populationSize * numberOfCities];		  // tablica populacji 1D
////////////////////////////////////////////////////////////////

__constant__ int Distances[(numberOfCities + 1)*(numberOfCities)];


__global__ void CalculateDistance(const int* population, int* result)
{
	long tmpDistance = 0;
	int fromCity, toCity = 0;
	int tid = blockIdx.x;// *blockDim.x + threadIdx.x;
	int firstCity = tid * numberOfCities;
	for (int i = 0; i < numberOfCities - 1; i++)             ////////// odleg³osci od pierwszego do ostatniego
	{
		fromCity = population[ firstCity + i];
		toCity = population[ firstCity + i + 1];
		tmpDistance += Distances[ fromCity * numberOfCities + toCity];
	}
	fromCity = population[ (tid + 1) * numberOfCities - 1];        ///////// odleg³oœæ z ostatniego do pocz¹tkowego
	toCity = population[ firstCity];
	tmpDistance += Distances[ fromCity * numberOfCities + toCity];
	result[tid] = tmpDistance;
}

__global__ void reorderByKey(int* population, int* tmp_population, int* values)
{
	int tid = blockIdx.x * blockDim.x + threadIdx.x;
	int moveFrom = values[blockIdx.x] * numberOfCities;
	population[tid] = tmp_population[moveFrom + threadIdx.x];
}

__global__ void crossover(int *population)
{
	int tid = blockIdx.x * blockDim.x + threadIdx.x;
	int offset = numberOfCities*populationSize / 2;
	if (blockIdx.x % 2 == 0)        // parzyste miasta
	{
		if (threadIdx.x < numberOfCities / 2)
			population[tid + offset] = population[tid];
		else
			population[tid + offset + numberOfCities] = population[tid];
	}
	else
	{
		if (threadIdx.x < numberOfCities / 2)
			population[tid + offset] = population[tid];
		else
			population[tid + offset - numberOfCities] = population[tid];

	}
}

__global__ void normalizacja(int *population)
{
	int tid = blockIdx.x * blockDim.x + threadIdx.x;
	if (populationSize)
	{
		int tempArray[numberOfCities] = { 0 }; 
		int id = tid*numberOfCities;
		int city = 0;
		int counter = 0;
		for (int i = 0; i < numberOfCities; i++)
		{
			city = population[id+i];
			if (tempArray[city] == 0)
				tempArray[city] = 1;
			else
			{
				population[id+i] = -1;
				counter++;
			}
		}
		for (int i = 0; i < counter; i++)
		{
			int PositionA = 0, PositionB = 0;
			for (int j = PositionA; j < numberOfCities ; j++)
			{
				if (tempArray[j] == 0)
				{
					PositionA = j;
					tempArray[j] = 1;
					break;
				}
			}
			for (int j = PositionB; j < numberOfCities; j++)
			{
				if (population[id + j] == -1)
				{
					population[id + j] = PositionA;
					PositionB = j;
					break;
				}
			}
		}

	}
	

	for (int i = 0; i < numberOfCities; i++)
	{
		//int city = populationArray[]     
		/*if (tempArray[i] == 0)
			tempArray[i] = 1;
		else*/
			
	}
}

///// prototypy funkcji /////////////////
int* cudaCalculateResult(int *, int );
int* gpuSort(int*, int **);
void findDuplicates(int*);

void fillCitiesArray()
{
	ifstream fin;
	fin.open(fileName);
	if (fin.is_open())
	{
		string currentLine;
		string tmp;


		citiesArray[0][0] = 0;
		citiesArray[0][1] = 0;
		citiesArray[0][2] = 0;

		int count = 1;
		while (count < numberOfCities + 1)

		{
			fin >> citiesArray[count][0];
			fin >> citiesArray[count][1];
			fin >> citiesArray[count][2];
			count++;
		}
	}
}

void fillCitiesDistance()
{
	int distance, disX, disY;
	citiesDistance[0] = 0;        // pêtla nie obejmuje, niepotrzebne ustawione na 0
	for (int i = 1; i < numberOfCities + 1; i++)
	{
		citiesDistance[i] = 0;   //niepotrzebne komórki ustawione na 0
		citiesDistance[i*numberOfCities] = 0;
		for (int j = 1; j < numberOfCities; j++)
		{
			disX = _Pow_int(citiesArray[i][1] - citiesArray[j][1], 2);
			disY = _Pow_int(citiesArray[i][2] - citiesArray[j][2], 2);
			distance = (int)sqrt(disX + disY);
			citiesDistance[i*(numberOfCities)+j] = distance;
		}
	}
}

int myRandom(int i) { return std::rand() % i; }

void addGenom(int position)
{
	int idx = position * numberOfCities;
	for (int i = 0; i < numberOfCities; i++)
	{
		populationArray[idx + i] = i + 1;  // wpisanie numerów miast do genomu
	}
		random_shuffle(&populationArray[idx], &populationArray[idx + numberOfCities], myRandom);
		
}

void mutatePopulation()
{
	for (int i = 1; i < populationSize; i++)
	{
		int rand1 = std::rand()%numberOfCities;
		int rand2;
		do
		{
			rand2 = rand()%numberOfCities;
		} while (rand1==rand2);
		int id = i*numberOfCities;
		int temp = populationArray[id + rand1];
		populationArray[id + rand1] = populationArray[id + rand2];
		populationArray[id + rand2] = temp;
	}
}

void cudaInitialization()
{
	cudaError_t cudaSucces = cudaMemcpyToSymbol(Distances, citiesDistance, sizeof(citiesDistance));
	if (cudaSucces != cudaSuccess)
	{
		printf("Blad kopiowania do pamieci gpu");
	}
}    // kopiuje odleg³oœæi pomiêdzy miastami do pamiêci sta³ej GPU

int main()
{
	std::srand(unsigned(std::time(0)));
	fillCitiesArray();
	fillCitiesDistance();
	for (int i = 0; i < populationSize; i++)
	{
		addGenom(i);
	}
	cudaInitialization();
	/// inicjalizacja zakoñczona

	int result[populationSize];
	
	cudaCalculateResult(result, iterations);
	
	cout << result[0] << endl;
	return 0;
}

//void normalizacja()
//{
//	for (int i = populationSize/2; i < populationSize; i++)         //sprawdzenie konieczne jedynie dla drugiej czêœci populacji
//	{
//		int tmpCity = 0;
//		int temp[numberOfCities];
//		
//		for (int j = 0; j < numberOfCities; j++)
//		{
//			temp[j] = -2;                            // -2 oznacza brak w miasta w tablicy
//		}
//
//		int id = i * numberOfCities;                // pocz¹tek genomu w tablicy populacji
//		for (int k = 0; k < numberOfCities; k++)
//		{
//			tmpCity = populationArray[id + k];
//			temp[tmpCity-1] == -2 ? temp[tmpCity-1] = -1 : temp[tmpCity-1] = k;
//		} // dzia³a ok do tego miejsca
//		for (int i = 0; i < numberOfCities; i++)
//		{
//			int position = 0;
//			for (int j = 0; j < numberOfCities; j++)
//			{
//				if (temp[j]==-2)
//				{
//					tmpCity = j + 1;
//					break;
//				}
//			}
//			for (int k = 0; k < numberOfCities; k++)
//			{
//				if (temp[k]>-1)
//				{
//					populationArray[id + temp[k]] = tmpCity;
//					temp[tmpCity-1] = -1;
//					temp[k] = -1;
//					break;
//				}
//			}
//		}
//	}
//}

int* cudaCalculateResult(int * result, int iter)
{
	static int firstUse;
	static int keys[populationSize];
	static int * dev_populationArray;
	static int * dev_result;
	static int * p_tmp;

	if (!firstUse)
	{
		checkCudaErrors(cudaMalloc((void **)&dev_populationArray, sizeof(populationArray)));
		checkCudaErrors(cudaMalloc((void**)&dev_result, populationSize*sizeof(int)));
		firstUse = 1;
	}
	////////////////////////////////// g³owna pêtla programu
	for (int i = 0; i < iter; i++)
	{
		checkCudaErrors(cudaMemcpy(dev_populationArray, populationArray, sizeof(populationArray), cudaMemcpyHostToDevice));

		CalculateDistance << <populationSize, 1 >> >(dev_populationArray, dev_result);
		cudaMemcpy(keys, dev_result, populationSize*sizeof(int), cudaMemcpyDeviceToHost);

		p_tmp = gpuSort(keys, &dev_populationArray);

		crossover << <populationSize / 2, numberOfCities >> >(dev_populationArray);
		cudaDeviceSynchronize();

		normalizacja << <(populationSize + 127) / 128, 128 >> >(dev_populationArray);
		cudaMemcpy(populationArray, dev_populationArray, sizeof(populationArray), cudaMemcpyDeviceToHost);

		if (i%10==0)
		findDuplicates(keys);

		mutatePopulation();

		cout << keys[0] << endl;
	}
	///////////////////////////////////// kopiowanie wynikow "na zewnatrz" i finalizacja
	for (int i = 0; i < populationSize; i++)
	{
		result[i] = keys[i];
	}
	
	cudaFree(dev_populationArray);
	cudaFree(dev_result);
	cudaFree(p_tmp);
	return result;
}

int* gpuSort(int* keys, int **popAray)   // keys - oceny populacji
{
	static int* dev_values;
	static int noFirst;
	static int* tmp_population;
	static int values[populationSize];

	for (int i = 0; i < populationSize; i++)
	{
		values[i] = i;
	}
	
	thrust::sort_by_key(keys, keys + populationSize, values);

	if (!noFirst)
	{
		checkCudaErrors(cudaMalloc((void**)&tmp_population, sizeof(populationArray)));
		checkCudaErrors(cudaMalloc((void**)&dev_values, populationSize*sizeof(int)));
		noFirst = 1;
	}
	
	checkCudaErrors(cudaMemcpy(dev_values, values, populationSize*sizeof(int), cudaMemcpyHostToDevice));
	checkCudaErrors(cudaMemcpy(tmp_population, *popAray, sizeof(populationArray), cudaMemcpyDeviceToDevice));
	cudaDeviceSynchronize();

	reorderByKey << <populationSize, numberOfCities >> >(*popAray, tmp_population, dev_values); 
	cudaDeviceSynchronize();
	return tmp_population;
}

void findDuplicates(int* keys)
{
	for (int i = 0; i < populationSize-1; i++)
	{
		if (keys[i] == keys[i + 1])
		{
			addGenom(i + 1);
		}
	}
}

