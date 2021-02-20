import random
import itertools


def add_edge(inputGraph, vertex, vertexTo):
    inputGraph[vertex].append(vertexTo)
    inputGraph[vertexTo].append(vertex)


def add_vertex(inputGraph, vertex):
    inputGraph[vertex] = []


def createRandomGraph(Vertex, Edges):
    graph = {}
    for i in range(Vertex):
        add_vertex(graph, i)

    for _ in range(Edges):
        selectRandomIndex = random.randint(0, Vertex - 1)
        while len(graph[selectRandomIndex]) == Vertex - 1:
            selectRandomIndex = random.randint(0, Vertex - 1)
        selectRandomIndex2 = random.randint(0, Vertex - 1)
        while selectRandomIndex2 == selectRandomIndex or selectRandomIndex2 in graph[selectRandomIndex]:
            selectRandomIndex2 = random.randint(0, Vertex - 1)
        add_edge(graph, selectRandomIndex, selectRandomIndex2)
    return graph


def heuristicLongestCycle(graph, V):
    visitedDict = {}
    parents = []
    for vertex in graph:
        visitedDict[vertex] = 0
        parents.append(-1)

    randomRootNodes = list(range(V))
    random.shuffle(randomRootNodes)
    cycleStartEnd = []
    isCycle = False

    for randomRoot in randomRootNodes:
        if visitedDict[randomRoot] == 0:
            isCycle = dfs(randomRoot, parents[randomRoot], parents, visitedDict, graph, cycleStartEnd)
            if isCycle:
                break
    if isCycle:
        cycleStart, cycleEnd = cycleStartEnd[0], cycleStartEnd[1]

        cycle = [cycleStart]
        v = cycleEnd

        while v != cycleStart:
            cycle.append(v)
            v = parents[v]

        cycle.append(cycleStart)
        return cycle
    else:
        return None


def dfs(v, p, parents, visitedNodes, graph, cycleStartEnd):
    visitedNodes[v] = 1
    for neighbour in graph[v]:
        if neighbour != p:
            if visitedNodes[neighbour] == 0:
                parents[neighbour] = v
                if dfs(neighbour, v, parents, visitedNodes, graph, cycleStartEnd):
                    return True
            elif visitedNodes[neighbour] == 1:
                cycleStartEnd.append(neighbour)
                cycleStartEnd.append(v)
                return True

    visitedNodes[v] = 2
    return False


def exactLongestCycle(graph, V):
    vertex = list(range(V))
    longestCycle = []
    globalCycle = False
    for size in range(V, 2, -1):
        allPermutations = list(itertools.permutations(vertex, size))
        for eachPermutation in allPermutations:
            eachPermutationList = list(eachPermutation)
            isCycle = True
            for index in range(len(eachPermutationList)):
                nextIndex = index + 1
                if index == len(eachPermutationList) - 1:
                    nextIndex = 0

                currentNode = eachPermutationList[index]
                nextNode = eachPermutationList[nextIndex]
                if nextNode not in graph[currentNode]:
                    isCycle = False
                    break
            if isCycle:
                eachPermutationList.append(eachPermutationList[0])
                longestCycle = eachPermutationList.copy()
                globalCycle = True
                break
        if globalCycle:
            break

    return longestCycle


V, E = 5, 7

graph1 = createRandomGraph(V, E)

print(graph1)

heuristicResult = heuristicLongestCycle(graph1, V)
print(50 * "*")
print(heuristicResult)
exactResult = exactLongestCycle(graph1, V)
print(exactResult)



