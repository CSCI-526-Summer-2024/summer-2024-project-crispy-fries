
library(gridExtra)

library(googlesheets4)

# Explicitly specify the scopes
scopes <- c(
  "https://www.googleapis.com/auth/spreadsheets",  # for reading and writing Google Sheets
  "https://www.googleapis.com/auth/drive"           # if you need access to Google Drive
)


gs4_deauth() 
gs4_auth(scopes = scopes)

sheet_url = "https://docs.google.com/spreadsheets/d/1zKLYZq8zkGUDxTR48Jd24c4RQ4_VVgxisQf7dF7kiGA/edit?resourcekey=&gid=1465979568#gid=1465979568"
data <- read_sheet(sheet_url)





library(tidyverse)
# test1 = data %>% filter(PassLevelOrDie == "Dead")
# test1[,1]

Beta_prog_build_time <- as.Date("2024-6-28")
Beta_build_time <- as.Date("2024-6-26")
Alpha_prog_build_time <- as.Date("2024-6-21")
Alpha_build_time <- as.Date("2024-6-19")


Alpha <- data %>% filter(Timestamp <= Alpha_build_time)
Alpha_prog <-data %>% filter(Timestamp<= Alpha_prog_build_time & Timestamp > Alpha_build_time)
Beta <- data %>% filter(Timestamp <= Beta_build_time & Timestamp > Alpha_prog_build_time)
Beta_prog_build_time <- data %>% filter(Timestamp <= Beta_prog_build_time & Timestamp > Beta_build_time)









graph_and_analysis <-function(x) {
  
  ## 1 Filter ratioLight with correct setting and draw heat map
  data1inFun <- x %>%
    mutate(RatioLight1 = `Time in light area of disabled light 1` / Light1TimeOff,
           RatioLight2 = `Time in light area of disabled light 2` / Light2TimeOff,
           RatioLight3 = `Time in light area of disabled light 3` / Light3TimeOff,
           RatioLight4 = `Time in light area of disabled light 4` / Light4TimeOff,
           RatioLight5 = `Time in light area of disabled light 5` / Light5TimeOff,
           RatioLight6 = `Time in light area of disabled light 6` / Light6TimeOff)  %>%   filter(RatioLight1 < 1 | is.na(RatioLight1) | is.nan(RatioLight1) , RatioLight2 < 1 | is.na(RatioLight2) | is.nan(RatioLight2) , RatioLight3 < 1 | is.na(RatioLight3) | is.nan(RatioLight3) , RatioLight4 < 1 | is.na(RatioLight4) | is.nan(RatioLight4) , RatioLight5 < 1 | is.na(RatioLight5) | is.nan(RatioLight5) , RatioLight6 < 1| is.na(RatioLight6) | is.nan(RatioLight6) )
  
  
  # data %>% filter(Timestamp == as.Date("2024-6-22"))
  
  library(ggplot2)
  library(reshape2)
  # draw graph heatmap for ratio in different scene
  melted_data <- melt(data1inFun, id.vars = "Scene Number", measure.vars = c("RatioLight1", "RatioLight2", "RatioLight3", "RatioLight4","RatioLight5", "RatioLight6"))
  p1 <- ggplot(melted_data, aes(x = variable, y = `Scene Number`, fill = value)) +
    geom_tile() +
    scale_fill_gradient(low = "blue", high = "red") +
    labs(title = "Heatmap of Time Ratios in Disabled Light Areas", x = "Light Area", y = "Scene")
  
  ## 2 Number of Times Caught by Spotlights
  death_counts <- x %>%
    group_by(DeadtoLight) %>%
    summarize(Deaths = n())
  
  p2 <- ggplot(death_counts, aes(x = DeadtoLight, y = Deaths)) +
    geom_bar(stat = "identity", fill = "tomato") +
    labs(title = "Deaths by Spotlight", x = "Light", y = "Number of Deaths")
  
  

  
  
  
  ## Metric #3: Route tracking via checkpoints
  checkpoint_list <- x %>%
    mutate(AllCheckPointPassed = strsplit(AllCheckPointsPassed, ",")) %>%
    unnest(AllCheckPointPassed)
  
  checkpoint_counts <- checkpoint_list %>%
    group_by(`Scene Number`, AllCheckPointPassed) %>%
    summarise(Count = n(), .groups = 'drop')
  
  
  p3 <- ggplot(checkpoint_counts, aes(x = AllCheckPointPassed, y = Count, fill = as.factor(`Scene Number`))) +
    geom_bar(stat = "identity", position = position_dodge()) +
    facet_wrap(~`Scene Number`) +
    labs(title = "Checkpoint Counts by Scene Number", x = "Checkpoint", y = "Count")
  
  # Metric #4: Time Spent in Shadow and Normal Form
  # data_for_metric4 <- x %>% filter()
  form_times <- x %>%
    group_by(`Scene Number`) %>%
    summarise(AvgShadowFormTime = mean(ShadowFormTime), AvgNormalFormTime = mean(NormalFormTime))
  
  
  p4 <- ggplot(form_times, aes(x = `Scene Number`, y = AvgShadowFormTime, fill = "Shadow Form")) +
    geom_bar(stat = "identity") +
    geom_bar(aes(y = AvgNormalFormTime, fill = "Normal Form"), stat = "identity") +
    labs(title = "Average Time Spent in Forms per Level", x = "Level", y = "Average Time")
  
  grid.arrange(p1, p2, p3,p4,  nrow = 2)
  
}




graph_and_analysis(Beta_prog_build_time)
























