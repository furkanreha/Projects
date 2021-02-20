import random

record_writer = open("report.txt", "w")
"""******************************************************************************************************************"""
action_size = int(input("Enter the Action Size: "))
actions = []
string_actions = ""

for i in range(0, action_size):
    actions.append("A" + str(i))
    string_actions += "A" + str(i) + " "

record_writer.write("Actions: " + string_actions + "\n")
print("Actions:", string_actions)
"""******************************************************************************************************************"""
ids_size = int(input("Enter the User Size: "))
ids = []
string_ids = ""

for i in range(1, ids_size + 1):
    ids.append(i)
    string_ids += str(i) + " "

record_writer.write("Ids: " + string_ids + "\n")
print("Ids:", string_ids)
"""******************************************************************************************************************"""
days = ["M", "T", "R", "TH", "F"]
string_days = ""

for element in days:
    string_days += element + " "

record_writer.write("Days: " + string_days + "\n")
print("Days:", string_days)
"""******************************************************************************************************************"""
periods = ["9-12", "1-3", "3-5"]
string_periods = ""

for element in periods:
    string_periods += element + " "

record_writer.write("Periods: " + string_periods + "\n")
print("Periods: ", string_periods, "\n")
"""******************************************************************************************************************"""
user_bias_actions_lower_limit = int(input("Enter the Bias Lower Limit for User and [Actions]: "))
user_bias_actions_upper_limit = int(input("Enter the Bias Upper Limit for User and [Actions]: "))
user_bias_actions = {}
string_user_bias_actions = ""

for id in ids:
    string_user_bias_actions += "(" + str(id) + ": "
    bias_array = []
    bias_array_size = random.randint(user_bias_actions_lower_limit, user_bias_actions_upper_limit)
    while len(bias_array) < bias_array_size:
        number = random.randint(0, len(actions) - 1)
        if number not in bias_array:
            bias_array.append(number)
            string_user_bias_actions += str(actions[number]) + " "
    user_bias_actions[id] = bias_array
    string_user_bias_actions = string_user_bias_actions[0:len(string_user_bias_actions) - 1] + ") "

record_writer.write("User-Bias-[Action]: " + string_user_bias_actions + "\n")
print("User-Bias-[Action]:", string_user_bias_actions)
"""******************************************************************************************************************"""
action_bias_days = {}
string_action_bias_days = ""
for action in actions:
    string_action_bias_days += "(" + action + ": "
    action_bias_days[action] = random.randint(0, 4)
    string_action_bias_days += days[action_bias_days[action]] + ") "

record_writer.write("Action-Bias-Day: " + string_action_bias_days + "\n")
print("Action-Bias-Day:", string_action_bias_days)
"""******************************************************************************************************************"""
action_bias_period = {}
string_action_bias_period = ""
for action in actions:
    string_action_bias_period += "(" + action + ": "
    action_bias_period[action] = random.randint(0, len(periods) - 1)
    string_action_bias_period += periods[action_bias_period[action]] + ") "

record_writer.write("Action-Bias-Period: " + string_action_bias_period + "\n")
print("Action-Bias-Period:", string_action_bias_period)
"""******************************************************************************************************************"""
file_writer = open("sample.txt", "w")
file_writer.write("id action day period\n")
data_size = int(input("\nEnter the Data Size: "))
print()
"""******************************************************************************************************************"""
print("Probability of an Action to Occur (Without Bias):", "{:.2%}".format(1/len(actions)))
random_lower_limit_bias_actions = int(input("Enter the Lower Bias Percentage for User-Bias-[Actions] between %0 "
                                            "and %100: "))
random_upper_limit_bias_actions = int(input("Enter the Upper Bias Percentage for User-Bias-[Actions] between %0 "
                                            "and %100: "))

all_randoms_bias_actions = {}
for id in ids:
    all_randoms_bias_actions[id] = random.randint(random_lower_limit_bias_actions, random_upper_limit_bias_actions)
print("Bias Percentage Values for each Id (Bias btw Id-[Actions]")
print(all_randoms_bias_actions, "\n")
"""******************************************************************************************************************"""
print("Probability of an Day to Occur (Without Bias):", "{:.2%}".format(1/len(days)))
random_lower_limit_bias_day = int(input("Enter the Lower Bias Percentage for Action-Bias-Day between %0 and %100: "))
random_upper_limit_bias_day = int(input("Enter the Upper Bias Percentage for Action-Bias-Day between %0 and %100: "))

all_randoms_bias_day = {}
for action in actions:
    all_randoms_bias_day[action] = random.randint(random_lower_limit_bias_day, random_upper_limit_bias_day)
print("Bias Percentage Values for each Action (Bias btw Action-Day)")
print(all_randoms_bias_day, "\n")
"""******************************************************************************************************************"""
print("Probability of an Period to Occur (Without Bias):", "{:.2%}".format(1/len(periods)))
random_lower_limit_bias_period = int(input("Enter the Lower Bias Percentage for Action-Bias-Period between %0 "
                                           "and %100: "))
random_upper_limit_bias_period = int(input("Enter the Upper Bias Percentage for Action-Bias-Period between %0 "
                                           "and %100: "))

all_randoms_bias_period = {}
for action in actions:
    all_randoms_bias_period[action] = random.randint(random_lower_limit_bias_period, random_upper_limit_bias_period)
print("Bias Percentage Values for Each Action (Bias btw Action-Period)")
print(all_randoms_bias_period)
"""******************************************************************************************************************"""
actual_data = []
for i in range(0, data_size):
    row = ""
    user_random = random.randint(1, len(ids))
    row += str(user_random)

    action_bias_percentage_random = all_randoms_bias_actions[user_random]
    random_percentage = random.randint(0, 100)
    action = ""
    action_bias_array = user_bias_actions[user_random]
    if random_percentage <= action_bias_percentage_random:
        action = actions[action_bias_array[random.randint(0, len(action_bias_array)) - 1]]
    else:
        action_index = action_bias_array[0]
        while action_index in action_bias_array:
            action_index = random.randint(0, len(actions) - 1)
        action = actions[action_index]

    row += " " + str(action)

    day_bias_percentage_random = all_randoms_bias_day[action]
    random_percentage = random.randint(0, 100)
    day_name = ""
    if random_percentage <= day_bias_percentage_random:
        day_index = action_bias_days[action]
        day_name = days[day_index]
    else:
        day_index = action_bias_days[action]
        day_index_bias = action_bias_days[action]
        while day_index == day_index_bias:
            day_index = random.randint(0, len(days) - 1)
        day_name = days[day_index]
    row += " " + str(day_name)

    period_bias_percentage_random = all_randoms_bias_period[action]
    random_percentage = random.randint(0, 100)
    period = ""
    if random_percentage <= period_bias_percentage_random:
        period_index = action_bias_period[action]
        period = periods[period_index]
    else:
        period_index = action_bias_period[action]
        period_index_bias = action_bias_period[action]
        while period_index == period_index_bias:
            period_index = random.randint(0, len(periods) - 1)
        period = periods[period_index]
    row += " " + str(period)

    actual_data.append(row)
    file_writer.write(row + "\n")

"""******************************************************************************************************************"""
action_total = {}
bias_period_total = {}
bias_day_total = {}

id_total = {}
bias_id_total = {}

for e in actual_data:
    data_array = e.split(" ")

    id = int(data_array[0])
    action = data_array[1]

    if id not in id_total:
        id_total[id] = 1
    else:
        id_total[id] += 1

    bias_actions_index = user_bias_actions[id]
    bias_actions = []
    for index in bias_actions_index:
        bias_actions.append(actions[index])

    if action in bias_actions:
        if id not in bias_id_total:
            bias_id_total[id] = 1
        else:
            bias_id_total[id] += 1

    if action not in action_total:
        action_total[action] = 1
    else:
        action_total[action] += 1

    bias_day = days[action_bias_days[action]]
    actual_day = data_array[2]
    if bias_day == actual_day:
        if action not in bias_day_total:
            bias_day_total[action] = 1
        else:
            bias_day_total[action] += 1

    bias_period = periods[action_bias_period[action]]
    actual_period = data_array[3]
    if actual_period == bias_period:
        if action not in bias_period_total:
            bias_period_total[action] = 1
        else:
            bias_period_total[action] += 1
"""******************************************************************************************************************"""

record_writer.write("\nBias Ratio Report btw Id-Action\n")
record_writer.write("***********************************\n")
for i in ids:
    total_occurrence = id_total[i]
    total_bias = bias_id_total[i]
    s_actions = ""
    for a in user_bias_actions[i]:
        s_actions += actions[a] + " "
    record_writer.write("Action-Bias-Ratio for " + str(i) + " and " + str(s_actions)[0:len(s_actions) - 1] + ": " +
                        str("{:.2%}".format(total_bias / total_occurrence)) + "\n")
record_writer.write("***********************************\n")

"""******************************************************************************************************************"""

record_writer.write("\nBias Ratio Report btw Action-Day\n")
record_writer.write("***********************************\n")
for a in actions:
    total_occurrence = action_total[a]
    total_bias = bias_day_total[a]
    record_writer.write("Day-Bias-Ratio for " + str(a) + " and " + str(days[action_bias_days[a]]) + ": " +
                        str("{:.2%}".format(total_bias / total_occurrence)) + "\n")
record_writer.write("***********************************\n")

record_writer.write("\nBias Ratio Report btw Action-Period\n")
record_writer.write("***********************************\n")
for a in actions:
    total_occurrence = action_total[a]
    total_bias = bias_period_total[a]
    record_writer.write("Period-Bias-Ratio for " + str(a) + " and " + str(periods[action_bias_period[a]]) + ": "
                        + str("{:.2%}".format(total_bias / total_occurrence)) + "\n")
record_writer.write("***********************************\n")

"""******************************************************************************************************************"""



