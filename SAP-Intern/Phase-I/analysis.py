file_reader = open("result.txt", "r")
result_data = file_reader.readlines()

file_reader = open("report.txt", "r")
all_file = file_reader.readlines()

action_bias_day_data = all_file[5][18:].split("(")
action_day_dic = {}
for i in range(0, len(action_bias_day_data)):
    array = action_bias_day_data[i][0:len(action_bias_day_data[i]) - 2].split(" ")
    action_day_dic[array[0].replace(":", "")] = array[1].replace(")", "")

matching_day = 0

for e in result_data:
    array = e.split(" ")
    action = array[1].replace("\"", "")
    day = array[2].replace("\"", "")
    expected_day = action_day_dic[action]
    if day == expected_day:
        matching_day += 1

print("Day Matching Ratio:", "{:.2%}".format(matching_day / len(result_data)))

action_bias_period_data = all_file[6][21:].split("(")
action_period_dic = {}

for i in range(0, len(action_bias_period_data)):
    array = action_bias_period_data[i][0:len(action_bias_period_data[i]) - 2].split(" ")
    action_period_dic[array[0].replace(":", "")] = array[1].replace(")", "")

matching_period = 0

for e in result_data:
    array = e.split(" ")
    action = array[1].replace("\"", "").strip()
    period = array[3].replace("\"", "").strip()
    expected_period = action_period_dic[action]
    if period == expected_period:
        matching_period += 1

print("Period Matching Ratio:", "{:.2%}".format(matching_period / len(result_data)))
