import os.path

import DijkstraAlg
from ortools.constraint_solver import routing_enums_pb2
from ortools.constraint_solver import pywrapcp


def create_data_model(Graph,ways,Route):
    """Stores the data for the problem."""
    data = {}
    flag=0
    data['distance_matrix']=[]
    dijkstra=DijkstraAlg.DijkstraMain(Graph)
    nodes=list()
    for i in Graph:
        nodes.append(i[0])
    for i in Route:
        temp=[]
        for u in Route:
            temp.append(dijkstra[i].get_distance(Graph[u][0]))
        data['distance_matrix'].append(temp)

    data['num_vehicles'] = 2
    data['depot'] = 0
    print(dijkstra[0].get_distance(Graph[6][0]))
    return data

def print_solution(data, manager, routing, solution,route,ways):
    """Prints solution on console."""
    max_route_distance = 0
    for vehicle_id in range(data['num_vehicles']):
        index = routing.Start(vehicle_id)
        plan_output = 'Route for vehicle {}:\n'.format(vehicle_id)
        route_distance = 0
        while not routing.IsEnd(index):
            temp=ways[route[manager.IndexToNode(index)]]
            plan_output += ' {} -> '.format(temp[len(temp)-1])
            previous_index = index
            index = solution.Value(routing.NextVar(index))
            route_distance += routing.GetArcCostForVehicle(
                previous_index, index, vehicle_id)
        temp=ways[route[manager.IndexToNode(index)]]
        plan_output += '{}\n'.format((temp[len(temp)-1]))
        plan_output += 'Distance of the route: {}m\n'.format(route_distance)
        print(plan_output)
        max_route_distance = max(route_distance, max_route_distance)
    print('Maximum of the route distances: {}m'.format(max_route_distance))


def main():
    
    mapPathStr="map3.osm"
    f = open(mapPathStr, "r",encoding="utf8")
    line=f.readline()
    ways=[]
    Route=[0,6,17,22]
    wayDescr=[]
    flag=False
    flag2=False
    tempStr=""
    while(line!=""):
        wayDescr=[]
        temp=line.split()
        if(temp[0] == "<way"):
            while(temp[0] != "</way>"):
                for k in line.split():
                    if(("id=" in k) and ("uid=" not in k)):
                        k=k.replace('id=','')
                        k=k.replace('"', '')
                        wayDescr.append(int(k))
                    elif("ref=" in k):
                        k=k.replace('ref=','')
                        k=k.replace('"', '')
                        k=k.replace('/>', '')
                        wayDescr.append(int(k)) 
                    #elif("oneway" in k):
                    #    k=k.replace('ref=','')
                    #    k=k.replace('"', '')
                    #    wayDescr.append("ONEWAY") 
                    elif("name" in k ):
                        flag=True
                    elif(flag):
                        k=k.replace('v="','')
                        k=k.replace('"', '')
                        if('/>' in k):
                            k=k.replace('/>', '')
                            tempStr=tempStr+k
                            wayDescr.append(tempStr)   
                            flag=False
                            flag2=True
                            tempStr=""
                        else:
                            tempStr=tempStr+k+' '
                line=f.readline()

                temp=line.split()
            if(flag2):
                ways.append(wayDescr)
                flag2=False
        else:
            line=f.readline()

    Graph=[]
    ConnectetWays=[]
    for i in ways:
        distance=0
        ConnectetWays.append(i[0])
        ConnectetWays.append(distance)
        for j in i:
            if(j!=i[0]): 
                distance=distance+1
                for k in ways:
                    if(k[0]!=i[0] and k[0] not in ConnectetWays):
                        for l in k:
                            if (j==l ):#and j!="ONEWAY"):
                                ConnectetWays.append(k[0])
                                ConnectetWays.append(distance)                               
                    
        Graph.append(ConnectetWays)
        ConnectetWays=[] 
    data=create_data_model(Graph,ways,Route)
    for i in data['distance_matrix']:
        print (i)
    # Create the routing index manager.
    manager = pywrapcp.RoutingIndexManager(len(data['distance_matrix']),
                                           data['num_vehicles'], data['depot'])

    # Create Routing Model.
    routing = pywrapcp.RoutingModel(manager)


    # Create and register a transit callback.
    def distance_callback(from_index, to_index):
        """Returns the distance between the two nodes."""
        # Convert from routing variable Index to distance matrix NodeIndex.
        from_node = manager.IndexToNode(from_index)
        to_node = manager.IndexToNode(to_index)
        return data['distance_matrix'][from_node][to_node]

    transit_callback_index = routing.RegisterTransitCallback(distance_callback)

    # Define cost of each arc.
    routing.SetArcCostEvaluatorOfAllVehicles(transit_callback_index)

    # Add Distance constraint.
    dimension_name = 'Distance'
    routing.AddDimension(
        transit_callback_index,
        0,  # no slack
        3000,  # vehicle maximum travel distance
        True,  # start cumul to zero
        dimension_name)
    distance_dimension = routing.GetDimensionOrDie(dimension_name)
    distance_dimension.SetGlobalSpanCostCoefficient(100)

    # Setting first solution heuristic.
    search_parameters = pywrapcp.DefaultRoutingSearchParameters()
    search_parameters.first_solution_strategy = (
        routing_enums_pb2.FirstSolutionStrategy.PATH_CHEAPEST_ARC)

    # Solve the problem.
    solution = routing.SolveWithParameters(search_parameters)

    # Print solution on console.
    if solution:
        print_solution(data, manager, routing, solution,Route,ways)

    print("EE")


if __name__ == '__main__':
    main()