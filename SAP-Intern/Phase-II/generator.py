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
minimum_frequency = int(input("Enter the Minimum Frequency (in Days): "))
maximum_frequency = int(input("Enter the Maximum Frequency (in Days): "))
string_frequencies = ""
for i in range(minimum_frequency, maximum_frequency + 1):
    string_frequencies += str(i) + " "

record_writer.write("Frequencies (in Days): " + string_frequencies + "\n")
print("Frequencies (in Days):", string_frequencies)
"""******************************************************************************************************************"""
periods = ["9-12", "1-3", "3-5"]
string_periods = ""

for element in periods:
    string_periods += element + " "

record_writer.write("Periods: " + string_periods + "\n")
print("Periods:", string_periods, "\n")
"""******************************************************************************************************************"""
user_bias_actions_lower_limit = int(input("Enter the Bias Lower Limit for User and [Actions]: "))
user_bias_actions_upper_limit = int(input("Enter the Bias Upper Limit for User and [Actions]: "))
user_bias_actions = {}
string_user_bias_actions = ""

for i in ids:
    string_user_bias_actions += "(" + str(i) + ": "
    bias_array = []
    bias_array_size = random.randint(user_bias_actions_lower_limit, user_bias_actions_upper_limit)
    while len(bias_array) < bias_array_size:
        number = random.randint(0, len(actions) - 1)
        if number not in bias_array:
            bias_array.append(number)
            string_user_bias_actions += str(actions[number]) + " "
    user_bias_actions[i] = bias_array
    string_user_bias_actions = string_user_bias_actions[0:len(string_user_bias_actions) - 1] + ") "

record_writer.write("User Bias [Actions]: " + string_user_bias_actions + "\n")
print("User Bias [Actions]:", string_user_bias_actions)
"""******************************************************************************************************************"""
action_bias_frequency = {}
action_bias_period = {}
string_action_bias_frequency = ""
string_action_bias_period = ""

for key in actions:
    random_number = random.randint(minimum_frequency, maximum_frequency)
    string_action_bias_frequency += "(" + key + " " + str(random_number) + ") "
    action_bias_frequency[key] = random_number
    random_number = random.randint(0, len(periods) - 1)
    string_action_bias_period += "(" + key + " " + periods[random_number] + ") "
    action_bias_period[key] = random_number

record_writer.write("Action Bias Frequency: " + string_action_bias_frequency + "\n")
print("Action Bias Frequency:", string_action_bias_frequency)
record_writer.write("Action Bias Period: " + string_action_bias_period + "\n")
print("Action Bias Period:", string_action_bias_period)
"""******************************************************************************************************************"""
file_writer = open("sample.txt", "w")
file_writer.write("id action freq period\n")
data_size = int(input("\nEnter the Data Size: "))
print()
"""******************************************************************************************************************"""
print("Probability of an Action to Occur (Without Bias):", "{:.2%}".format(1/len(actions)))

random_lower_limit_bias_actions = int(input("Enter the Lower Bias Percentage for User Bias [Actions] between %0 "
                                            "and %100: "))
random_upper_limit_bias_actions = int(input("Enter the Upper Bias Percentage for User Bias [Actions] between %0 "
                                            "and %100: "))

all_randoms_bias_actions = {}
for i in ids:
    all_randoms_bias_actions[i] = random.randint(random_lower_limit_bias_actions, random_upper_limit_bias_actions)
print("Bias Percentage Values for each Id (Bias btw Id and [Actions])")
print(all_randoms_bias_actions, "\n")
"""******************************************************************************************************************"""
print("Probability of an Frequency to Occur (Without Bias):", "{:.2%}".format(1/(maximum_frequency - minimum_frequency
                                                                                 + 1)))
random_lower_limit_bias_frequency = int(input("Enter the Lower Bias Percentage for Action Bias Frequency between "
                                              "%0 and %100: "))
random_upper_limit_bias_frequency = int(input("Enter the Upper Bias Percentage for Action Bias Frequency between "
                                              "%0 and %100: "))
all_randoms_bias_frequency = {}
for action in actions:
    all_randoms_bias_frequency[action] = random.randint(random_lower_limit_bias_frequency,
                                                        random_upper_limit_bias_frequency)
print("Bias Percentage Values for each Action (Bias btw Action Frequency)")
print(all_randoms_bias_frequency, "\n")
"""******************************************************************************************************************"""
print("Probability of an Period to Occur (Without Bias):", "{:.2%}".format(1/len(periods)))
random_lower_limit_bias_period = int(input("Enter the Lower Bias Percentage for Action Bias Period between %0 "
                                           "and %100: "))
random_upper_limit_bias_period = int(input("Enter the Upper Bias Percentage for Action Bias Period between %0 "
                                           "and %100: "))

all_randoms_bias_period = {}
for action in actions:
    all_randoms_bias_period[action] = random.randint(random_lower_limit_bias_period,
                                                     random_upper_limit_bias_period)
print("Bias Percentage Values for Each Action (Bias btw Action and Period)")
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

    frequency_bias_percentage_random = all_randoms_bias_frequency[action]
    random_percentage = random.randint(0, 100)
    frequency = ""
    if random_percentage <= frequency_bias_percentage_random:
        frequency = action_bias_frequency[action]
    else:
        frequency = action_bias_frequency[action]
        frequency_bias = frequency
        while frequency == frequency_bias:
            frequency = random.randint(minimum_frequency, maximum_frequency)

    row += " " + "P" + str(frequency)

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

    actual_data.append(row.replace("P", ""))
    file_writer.write(row + "\n")
"""******************************************************************************************************************"""
id_total = {}
action_total = {}

bias_period_total = {}
bias_id_total = {}
bias_frequency_total = {}

for e in actual_data:
    data_array = e.split(" ")

    i = int(data_array[0])
    action = data_array[1]

    if i not in id_total:
        id_total[i] = 1
    else:
        id_total[i] += 1

    bias_actions_index = user_bias_actions[i]
    bias_actions = []
    for index in bias_actions_index:
        bias_actions.append(actions[index])

    if action in bias_actions:
        if i not in bias_id_total:
            bias_id_total[i] = 1
        else:
            bias_id_total[i] += 1

    if action not in action_total:
        action_total[action] = 1
    else:
        action_total[action] += 1

    bias_frequency = action_bias_frequency[action]
    actual_frequency = int(data_array[2])
    if bias_frequency == actual_frequency:
        if action not in bias_frequency_total:
            bias_frequency_total[action] = 1
        else:
            bias_frequency_total[action] += 1

    bias_period = periods[action_bias_period[action]]
    actual_period = data_array[3]
    if actual_period == bias_period:
        if action not in bias_period_total:
            bias_period_total[action] = 1
        else:
            bias_period_total[action] += 1
"""******************************************************************************************************************"""
record_writer.write("\nBias Ratio Report btw Id and Action\n")
record_writer.write("***********************************************************************************************\n")
for i in ids:
    total_occurrence = id_total[i]
    total_bias = bias_id_total[i]
    s_actions = ""
    for a in user_bias_actions[i]:
        s_actions += actions[a] + " "
    record_writer.write("Action-Bias-Ratio for " + str(i) + " and " + str(s_actions)[0:len(s_actions) - 1] + ": " +
                        str("{:.2%}".format(total_bias / total_occurrence)) + "\n")
record_writer.write("***********************************************************************************************\n")
"""******************************************************************************************************************"""
record_writer.write("\nBias Ratio Report btw Action and Frequency (in days)\n")
record_writer.write("***********************************\n")
for a in actions:
    total_occurrence = action_total[a]
    total_bias = bias_frequency_total[a]
    record_writer.write("Frequency-Bias-Ratio for " + str(a) + " and " + str(action_bias_frequency[a]) +
                        "(in days)" + ": " + str("{:.2%}".format(total_bias / total_occurrence)) + "\n")
record_writer.write("***********************************************************************************************\n")
"""******************************************************************************************************************"""
record_writer.write("\nBias Ratio Report btw Action and Period\n")
record_writer.write("***********************************\n")
for a in actions:
    total_occurrence = action_total[a]
    total_bias = bias_period_total[a]
    record_writer.write("Period-Bias-Ratio for " + str(a) + " and " + str(periods[action_bias_period[a]]) +
                        ": " + str("{:.2%}".format(total_bias / total_occurrence)) + "\n")
record_writer.write("***********************************************************************************************\n")
"""******************************************************************************************************************"""
