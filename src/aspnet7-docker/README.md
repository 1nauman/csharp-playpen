# Links

AWS library's Github repository 
https://github.com/aws/aws-ssm-data-protection-provider-for-aspnet

# AWS Permissions needed for the application to read and write to parameter store

```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "rule1",
            "Effect": "Allow",
            "Action": [
                "ssm:PutParameter",
                "ssm:GetParametersByPath"
            ],
            "Resource": "*"
        }
    ]
}
```

# Commands

```shell
docker system prune -a <<< y && dotnet publish --os linux --arch x64 -c Release -p:PublishProfile=DefaultContainer

docker run -it --rm -p 5010:80 --name aspnet7-docker -e AWS_ACCESS_KEY_ID=${AWS_ACCESS_KEY_ID} -e AWS_SECRET_ACCESS_KEY=${AWS_SECRET_ACCESS_KEY} -e AWS_SESSION_TOKEN=${AWS_SESSION_TOKEN} -e AWS_REGION=ap-south-1 aspnet7-docker:1.0.0
```