#!/usr/bin/env python
"""

"""
import os
import sys
import re

### START COMMON

# matches any of the following without capturing
# 2243242.32412341
# 2342342.23412312.3423412341.234123412.341234123412341.123412
# 1.2.3.4.5.6.7
# 23523423
VERSION_REGEX = r"\d+(?:\.\d+)*"

NUGET_BETA_SUFFIX_REGEX = r"?:(-beta[1-8])?"
VALID_VERSION_REGEX = r"^\d+\.\d+\.\d+(?:|BETA[1-8]|SNAPSHOT)$"

def replace_guarded_pattern(file_name, pattern_string, replacement):
    contents = open(file_name).readlines()
    pattern = re.compile(pattern_string)
    output = []
    found = False
    def replacement_func(match):
        if len(match.groups()) == 4:
            return '{}{}{}'.format(match.group(1), replacement, match.group(4))
        else:
            return '{}{}{}'.format(match.group(1), replacement, match.group(2))

    for line in contents:
        line, subs = pattern.subn(replacement_func, line)
        found = found or subs > 0
        output.append(line)
    if not found:
        return False
    open(file_name, 'w').write(''.join(output))
    return True

def find(path, regex):
    ret = []
    def walker(ignore, dirname, filenames):
        for filename in filenames:
            file = os.path.join(dirname, filename)
            if re.search(regex, file):
                ret.append(file)
    os.path.walk(path, walker, [])
    return ret

def usage():
    print 'please specify a valid version, e.g. 4.3.0, or 4.3.0BETA2, or 4.3.0SNAPSHOT'
    print 'usage:'
    print '   %s <version>' % os.path.basename(__file__)
    sys.exit(-1)

# TODO: when we dont want to build versions before 5.1 we can remove 3 here
if len(sys.argv) != 2 and len(sys.argv) != 3:
    usage()
	
if not (re.match(VALID_VERSION_REGEX,sys.argv[1])):
	usage()

def version_to_fix(version):
    fix = "10"
    if (re.search(r"SNAPSHOT$",version)):
        fix = "1"
    beta = re.search(r"BETA([1-8])$",version)
    if (beta):
        fix = str(int(beta.group(1))+1)
    return fix

def get_nuget_beta_suffix(version):
    beta = re.search(r"BETA([1-8])$", version)
    if (beta):
        return beta.group().replace("BETA", "-beta")
    else:
        return ""

version = sys.argv[1]
version_without_suffix = re.sub(r'(BETA\d|SNAPSHOT)$', '', version)
major, minor, patch = version_without_suffix.split('.')
fix = version_to_fix(version)
version_nuget = "{}.{}.{}.{}".format(major,minor,patch,fix)
nuget_beta_suffix = get_nuget_beta_suffix(version)
suffix = version[len(version_without_suffix):]

print("""computed versions
    version={}
    version_without_suffix={}
    version_nuget={}
    nuget_beta_suffix={}
    major={}
    minor={}
    patch={}
    fix={}
    suffix={}""".format(version,version_without_suffix,version_nuget,nuget_beta_suffix,major,minor,patch,fix,suffix))

### END COMMON

def update_assembly_info(version):
    for file in find(".", "AssemblyInfo.cs$"):
        replace_guarded_pattern(file, r"""(AssemblyVersion\("){}("\))""".format(VERSION_REGEX), version)
        replace_guarded_pattern(file, r"""(AssemblyFileVersion\("){}("\))""".format(VERSION_REGEX), version)
        
def update_package_config_versions(version, nuget_beta_suffix):
    for file in find(".", "\.config$"):
        replace_guarded_pattern(file, r"""(<package id="Scandit\.[^"]*" version=")({})({})(")""".format(VERSION_REGEX, NUGET_BETA_SUFFIX_REGEX), version + nuget_beta_suffix)
        
def update_project_json_versions(version, nuget_beta_suffix):
    for file in find(".", "\.json$"):
        replace_guarded_pattern(file, r"""("Scandit\.(?:BarcodePicker(?:\.Unified)?|Recognition)\"\s*:\s*")({})({})(")""".format(VERSION_REGEX, NUGET_BETA_SUFFIX_REGEX), version + nuget_beta_suffix)
        replace_guarded_pattern(file, r"""("Scandit\.(?:BarcodePicker(?:\.Unified)?|Recognition)/)({})({})(")""".format(VERSION_REGEX, NUGET_BETA_SUFFIX_REGEX), version + nuget_beta_suffix)

def update_nuget_targets(version, nuget_beta_suffix):
    for file in find(".", "\.nuget\.targets"):
        replace_guarded_pattern(file, r"(Scandit\.(?:BarcodePicker|Recognition)\\)({})({})(\\)".format(VERSION_REGEX, NUGET_BETA_SUFFIX_REGEX), version + nuget_beta_suffix)

def update_windows_csproj_files(version, nuget_beta_suffix):
    for file in find(".", r"\.csproj$"):
        replace_guarded_pattern(file, r"(Scandit\.(?:BarcodePicker|Recognition)\.)({})({})()".format(VERSION_REGEX, NUGET_BETA_SUFFIX_REGEX), version + nuget_beta_suffix)
        refs = "|".join([
            "ScanditSDK",
            "VideoInputRT",
            "VideoInputRT.Interop",
            "Scandit.Recognition",
            "Scandit.BarcodePicker",
            "Scandit.BarcodePicker.Xamarin",
            "Scandit.BarcodePicker.Unified",
            "Scandit.BarcodePicker.Unified.Abstractions"])
        replace_guarded_pattern(file, r"""(<Reference Include="(?:{}),.*Version=){}()""".format(refs,VERSION_REGEX), version)
        replace_guarded_pattern(file, r"""(<HintPath>.*Scandit\.BarcodePicker\.(?:Xamarin|Unified)\.)({})({})()""".format(VERSION_REGEX, NUGET_BETA_SUFFIX_REGEX), version + nuget_beta_suffix)
        replace_guarded_pattern(file, r"""(<PackageReference Include="Scandit\.BarcodePicker\.Unified" Version=")({})({})(" ?/>)""".format(VERSION_REGEX, NUGET_BETA_SUFFIX_REGEX), version + nuget_beta_suffix)
        replace_guarded_pattern(file, r"""(<ReleaseVersion>){}(<\/ReleaseVersion>)""".format(VERSION_REGEX), version)

def update_sln(version):
    for file in find(".", r"\.sln$"):
        replace_guarded_pattern(file, "( *version *= *){}()".format(VERSION_REGEX), version)
        
### START REPLACING

update_assembly_info(version_nuget)
update_package_config_versions(version_nuget, nuget_beta_suffix)
update_project_json_versions(version_nuget, nuget_beta_suffix)
update_nuget_targets(version_nuget, nuget_beta_suffix)
update_windows_csproj_files(version_nuget, nuget_beta_suffix)
update_sln(version_nuget)

print("""successfully changed version numbers
You might want to commit the changes with:
git commit -a -m "bump version number to {}\"""".format(version))