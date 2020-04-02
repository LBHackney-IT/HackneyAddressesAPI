# INSTRUCTIONS: 
# 1) ENSURE YOU DELETE THE MODULES YOU DO NOT REQUIRE - E.G. YOU ONLY NEED A NETWORK LAYER, THEN DELETE REST OF THE MODULES (COMMON, BACK END, FRONT END)
# 2) ENSURE YOU REPLACE ALL INPUT PARAMETERS, THAT CURRENTLY STATE 'ENTER VALUE', WITH VALID VALUES 
# 3) YOUR CODE WOULD NOT COMPILE IF STEP NUMBER 2 IS NOT PERFORMED!
# 4) ENSURE YOU CREATE A BUCKET FOR YOUR STATE FILE AND YOU ADD THE NAME BELOW - MAINTAINING THE STATE OF THE INFRASTRUCTURE YOU CREATE IS ESSENTIAL
# 5) THE VALUES THAT ARE REPEATED ARE TAKEN OUT UNDER THE 'LOCALS' VARIABLE SECTION - PLEASE PROVIDE VALUES FOR THOSE

provider "aws" {
  region  = "eu-west-2"
  version = "~> 2.0"
}

locals {
  application_name = "address-api"
  vpc_name = "vpc-development-apis"
}


data "aws_iam_role" "ec2_container_service_role" {
  name = "ecsServiceRole"
}

data "aws_iam_role" "ecs_task_execution_role" {
  name = "ecsTaskExecutionRole"
}

terraform {
  backend "s3" {
    bucket  = "terraform-state-development-apis"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/address-api/state"
  }
}

module "development" {
  source                      = "github.com/LBHackney-IT/aws-hackney-components-per-service-terraform.git//modules/environment/backend/fargate"
  cluster_name                = "development-apis"
  ecr_name                    = "hackney/address-api"
  environment_name            = "development"
  application_name            = local.application_name 
  security_group_name         = "address-api"
  vpc_name                    = local.vpc_name
  host_port                        = 3000
  desired_number_of_ec2_nodes = 0
  lb_prefix                   = "nlb-development-apis"
  ecs_execution_role          = data.aws_iam_role.ecs_task_execution_role.arn
  lb_iam_role_arn             = data.aws_iam_role.ec2_container_service_role.arn
  task_definition_environment_variables = {
    ASPNETCORE_ENVIRONMENT = "development",
    ALLOWED_ADDRESSSTATUS_VALUES = "historical;alternative;approved preferred;provisional"
  }
  task_definition_environment_variable_count = 2
  cost_code                    = "B0811"
  task_definition_secrets      = {
    LLPGConnectionString = "/address-api/development/LLPGConnectionString"
  }
  task_definition_secret_count = 1
}

