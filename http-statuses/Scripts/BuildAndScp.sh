
if [[ -f Scripts/env.sh ]];
then
  source Scripts/env.sh
else
  echo 'no env file present'
  exit
fi

dotnet publish -v n && scp -i $keyfile -r ./bin/Debug/netcoreapp3.1/* $user@$ip:status-codes/
ssh -t -i $keyfile $user@$ip "sudo /bin/sh /var/www/status-codes.brakke.dev/UpdatePublish.sh" && tput setaf 2 && echo "Update successful."
